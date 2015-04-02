/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: ACAFormDesigner.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2010-2014
 * 
 *  Description:
 * 
 *  Notes:
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Accela.Controls.Designer.Data;
using System.Windows.Browser;
using Accela.ACA.FormDesigner.GFilterViewService;
using System.IO;
using System.Text;

namespace Accela.ACA.FormDesigner
{
    /// <summary>
    /// the class for ACAFormDesigner.
    /// </summary>
    public partial class ACAFormDesigner : UserControl
    {
        /// <summary>
        /// for store DataModelManager object.
        /// </summary>
        private DataModelManager dataModelManager = null;

        /// <summary>
        /// for store DataModelConverter object.
        /// </summary>
        private DataModelConverter dataModelConverter = null;

        /// <summary>
        /// for store SimpleViewModel4WS object.
        /// </summary>
        private SimpleViewModel4WS simpleViewModel4WS = null;

        /// <summary>
        /// for store bussiness parameter
        /// </summary>
        private BussinessParam bussinessParam = null;

        /// <summary>
        /// Initialize a new instance without load forminfo.
        /// </summary>
        public ACAFormDesigner()
        {
            InitializeComponent();
        }
        
        /// <summary>
        /// Initialize a new instance with business parameter and load data.
        /// </summary>
        /// <param name="param">BusinessParam object</param>
        public ACAFormDesigner(BussinessParam param)
        {
            InitializeComponent();

            dataModelManager = DataModelManager.GetDataModelManager(param);
            dataModelConverter = new DataModelConverter();
            dataModelManager.BusinessWebServiceObject.GetFilterScreenViewCompleted += new EventHandler<GetFilterScreenViewCompletedEventArgs>(BusinessWebServiceObject_getFilterScreenViewCompleted);
            dataModelManager.BusinessWebServiceObject.SaveFilterScreenViewCompleted += new EventHandler<System.ComponentModel.AsyncCompletedEventArgs>(BusinessWebServiceObject_saveFilterScreenViewCompleted);
            dataModelManager.Load(param.ViewId);
            this.formDesigner.Visibility = System.Windows.Visibility.Collapsed;
            this.messagePanel.ShowWaiting();
            this.bussinessParam = param;
            //HtmlPage.RegisterScriptableObject("FormDesignerSL", this);
            if (param.LangCode == "ar_AE")
            {
                this.FlowDirection = FlowDirection.RightToLeft;
            }
            else
            {
                this.FlowDirection = FlowDirection.LeftToRight;
            }
        }

        /// <summary>
        /// the method for initialize FormDesigner when load data completed.
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">getFilterScreenViewCompletedEventArgs object</param>
        void BusinessWebServiceObject_getFilterScreenViewCompleted(object sender, GetFilterScreenViewCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                this.formDesigner.Visibility = System.Windows.Visibility.Collapsed;
                ShowMessageByJS(bussinessParam.LoadingErrorMessge);
            }
            else
            {
                try
                {
                    this.formDesigner.Visibility = System.Windows.Visibility.Visible;
                    simpleViewModel4WS = e.Result;
                    if(string.IsNullOrEmpty(dataModelManager.ScreenPermissionModel4WS.permissionValue))
                    {
                        simpleViewModel4WS.permissionModel = dataModelManager.ScreenPermissionModel4WS;
                    }

                    if (simpleViewModel4WS != null)
                    {
                        FormInfo formInfo = dataModelConverter.ConvertToFormInfo(simpleViewModel4WS);
                        //string xmlstring = this.ToJson<FormInfo>(formInfo);
                        InitializeFormDesigner(formInfo);
                    }

                    this.messagePanel.Close();
                }
                catch (Exception)
                {
                    //this.messagePanel.ShowMessage(bussinessParam.LoadingErrorMessge);
                    ShowMessageByJS(bussinessParam.LoadingErrorMessge);
                }
            }
        }

        /// <summary>
        /// the method for do something when save data completed.
        /// </summary>
        /// <param name="sender">object of sender</param>
        /// <param name="e">AsyncCompletedEventArgs object</param>
        void BusinessWebServiceObject_saveFilterScreenViewCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                //this.messagePanel.ShowMessage(bussinessParam.SaveErrorMessge);
                ShowMessageByJS(bussinessParam.SaveErrorMessge);
                this.formDesigner.WhetherHasDesign = true;
            }
            else
            {
                try
                {
                    this.messagePanel.Close();
                    HtmlPage.Window.Invoke("ChangeRefreshTag", true);
                    HtmlPage.Window.Invoke("ChangeDataChangedTag", false);
                }
                catch (Exception)
                {
                    //this.messagePanel.ShowMessage(bussinessParam.SaveErrorMessge);
                    ShowMessageByJS(bussinessParam.SaveErrorMessge);
                }
            }
        }

        /// <summary>
        /// Initialize data in Form designer
        /// </summary>
        private void InitializeFormDesigner(FormInfo form)
        {
            this.formDesigner.EditForm(form);
            this.formDesigner.EventListener += new VDEventHandler(formDesigner_EventListener);
        }

        /// <summary>
        /// Event listenning handler
        /// </summary>
        /// <param name="componentType">a compoent type</param>
        /// <param name="e">VDEventArgs object</param>
        void formDesigner_EventListener(ComponentType componentType, VDEventArgs e)
        {
            if (componentType == Accela.Controls.Designer.Data.ComponentType.ToolBar && (ToolTypes)e.InternalObj == ToolTypes.Save) 
            {
                FormInfo result = e.ExternalObj as FormInfo;
                this.messagePanel.ShowWaiting();
                dataModelConverter.UpdateSimpleViewMode4WS(simpleViewModel4WS, result);
                dataModelManager.Save(simpleViewModel4WS);
            }

            if (componentType == Accela.Controls.Designer.Data.ComponentType.EelementDesignAction 
                || (componentType == ComponentType.ToolBar
                && ((ToolTypes)e.InternalObj == ToolTypes.AddSeparator || (ToolTypes)e.InternalObj == ToolTypes.DeleteSeparator 
                  || (ToolTypes)e.InternalObj== ToolTypes.SwitchDisplayLabel)))
            {
                try
                {
                    HtmlPage.Window.Invoke("ChangeDataChangedTag", true);
                }
                catch (Exception)
                { }
            }
        }

        /// <summary>
        /// method for show message by js
        /// </summary>
        /// <param name="message">message string</param>
        private void ShowMessageByJS(string message)
        {
            try
            {
                this.messagePanel.Close();
                HtmlPage.Window.Invoke("ShowErrorMessage", message);
            }
            catch (Exception)
            { }
        }

        /// <summary>
        /// for test
        /// </summary>
        //[ScriptableMember(ScriptAlias = "SavebyJs")]
        //public void test()
        //{
        //    MessageBox.Show("test js call silverlight!");
        //}

        /// <summary>
        /// for test
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        private string ToJson<T>(T obj)
        {
            System.Runtime.Serialization.DataContractSerializer ds = new System.Runtime.Serialization.DataContractSerializer(typeof(T));
            MemoryStream ms = new MemoryStream();
            ds.WriteObject(ms, obj);
            byte[] tempByte = ms.ToArray();
            string strJSON = Encoding.UTF8.GetString(ms.ToArray(), 0, tempByte.Length);
            ms.Close();
            return strJSON;
        }
    }
}
