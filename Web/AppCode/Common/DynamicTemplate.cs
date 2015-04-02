#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: DynamicTemplate.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2007-2014
 *
 *  Description:
 *  Interface define for admin.
 *
 *  Notes:
 * $Id: DynamicTemplate.cs 277071 2014-08-11 03:38:26Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;
using Accela.ACA.Common.Log;
using Accela.ACA.Common.Util;
using log4net;

namespace Accela.ACA.Web.Common
{
    /// <summary>
    /// It provides the dynamic template.
    /// </summary>
    public class DynamicTemplate : ITemplate
    {
        #region Fields

        /// <summary>
        /// Logger instance.
        /// </summary>
        private static readonly ILog Logger = LogFactory.Instance.GetLogger(typeof(DynamicTemplate));

        /// <summary>
        /// The list item type, ex: Header, Item.
        /// </summary>
        private readonly ListItemType _listItemType;

        /// <summary>
        /// The control that contains in the template.
        /// </summary>
        private readonly System.Web.UI.Control _control;

        #endregion Fields

        #region Construct Method

        /// <summary>
        /// Initializes a new instance of the DynamicTemplate class.
        /// </summary>
        /// <param name="type">The list item type.</param>
        /// <param name="control">The control that belong to the dynamic template.</param>
        public DynamicTemplate(ListItemType type, System.Web.UI.Control control)
        {
            _listItemType = type;
            _control = control;
        }

        #endregion Construct Method

        #region Properties

        /// <summary>
        /// Gets or sets the property name that bind to data source.
        /// </summary>
        public string BindPropertyName
        {
            get; 

            set;
        }

        /// <summary>
        /// Gets or sets the data expression specify the data source's data field that bind.
        /// </summary>
        public string BindDataExpression
        {
            get;

            set;
        }

        /// <summary>
        /// Gets or sets the specify id which control to bind.
        /// </summary>
        public string BindControlID
        {
            get;

            set;
        }

        #endregion Properties

        #region Method

        /// <summary>
        /// Defines the object that child controls and templates belong to. These child controls are in turn defined within an inline template.
        /// </summary>
        /// <param name="container">The object to contain the instances of controls from the inline template.</param>
        public void InstantiateIn(System.Web.UI.Control container)
        {
            switch (_listItemType)
            {
                case ListItemType.Header:
                    if (_control != null)
                    {
                        container.Controls.Add(_control);
                    }

                    break;
                case ListItemType.Item:
                    if (_control != null)
                    {
                        System.Web.UI.Control ctrl = CloneControl(_control);

                        PlaceHolder ph = new PlaceHolder();
                        ph.Controls.Add(ctrl);
                        ph.DataBinding += ItemDataBinding;

                        container.Controls.Add(ph);
                    }
                    
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Binding the item's data.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The event args.</param>
        private void ItemDataBinding(object sender, EventArgs e)
        {
            PlaceHolder ph = (PlaceHolder)sender;
            GridViewRow gridRow = (GridViewRow)ph.NamingContainer;

            try
            {
                object itemValue = DataBinder.Eval(gridRow.DataItem, BindDataExpression);
                System.Web.UI.Control ctrl = ph.FindControl(BindControlID);

                if (ctrl != null)
                {
                    System.Reflection.PropertyInfo pi = ctrl.GetType().GetProperty(BindPropertyName);
                    object convertedItemValue = Convert2RelatedDataType(itemValue, pi.PropertyType);

                    pi.SetValue(ctrl, convertedItemValue, null);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
            }
        }

        /// <summary>
        /// Converts the data to the related data type.
        /// </summary>
        /// <param name="value">The value want to convert.</param>
        /// <param name="convertedType">The converted type.</param>
        /// <returns>Return the value that converted desired type.</returns>
        private object Convert2RelatedDataType(object value, Type convertedType)
        {
            if (value == null || convertedType == value.GetType())
            {
                return value;
            }

            // convert datetime, double when the desired type is string.
            object convertedValue = value;
            if (convertedType == typeof(string))
            {
                convertedValue = string.Empty;

                if (value is DateTime)
                {
                    convertedValue = I18nDateTimeUtil.FormatToDateStringForUI((DateTime)value);
                }
                else if (value is double)
                {
                    convertedValue = I18nNumberUtil.FormatNumberForUI(value);
                }
                else if (value != DBNull.Value)
                {
                    convertedValue = Convert.ToString(value);
                }
            }

            return convertedValue;
        }

        /// <summary>
        /// Clone the control, CANNOT clone HtmlGenericControl.
        /// </summary>
        /// <param name="sourceCtrl">The source control.</param>
        /// <returns>Return a cloned instance of the control.</returns>
        private System.Web.UI.Control CloneControl(System.Web.UI.Control sourceCtrl)
        {
            Type t = sourceCtrl.GetType();
            object obj = Activator.CreateInstance(t);
            System.Web.UI.Control destCtrl = (System.Web.UI.Control)obj;
            PropertyDescriptorCollection srcPdc = TypeDescriptor.GetProperties(sourceCtrl);
            PropertyDescriptorCollection dstPdc = TypeDescriptor.GetProperties(destCtrl);

            for (int i = 0; i < srcPdc.Count; i++)
            {
                if (srcPdc[i].Attributes.Contains(DesignerSerializationVisibilityAttribute.Content))
                {
                    object srcValue = srcPdc[i].GetValue(sourceCtrl);
                    IList srcValueList = srcValue as IList;

                    if (srcValueList != null)
                    {
                        foreach (object child in srcValueList)
                        {
                            System.Web.UI.Control newChild = CloneControl(child as System.Web.UI.Control);

                            object dstValue = dstPdc[i].GetValue(destCtrl);
                            IList dstValueList = dstValue as IList;

                            if (dstValueList != null)
                            {
                                dstValueList.Add(newChild);
                            }
                        }
                    }
                }
                else
                {
                    dstPdc[srcPdc[i].Name].SetValue(destCtrl, srcPdc[i].GetValue(sourceCtrl));
                }
            }

            return destCtrl;
        }

        #endregion Method
    }
}