#region Header

/**
 * <pre>
 *
 *  Accela
 *  File: AccelaTextBoxWithImageButton.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 *
 *  Description:
 *  Appends the image button(search) to text control for search.
 *  
 *  Notes:
 * $Id: AccelaTextBox.cs 136242 2009-07-09 9:00 Solt.su $. 
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Accela.Web.Controls
{ 
    /// <summary>
    /// expand AccelaTextBox for search, add a ImageButton Control after text control
    /// </summary>
    public class AccelaTextBoxWithImageButton : AccelaTextBox
    {
        #region Fields

        /// <summary>
        /// Image Button
        /// </summary>
        private ImageButton _imageButton;

        #endregion

        #region constructed function

        /// <summary>
        /// Initializes a new instance of the AccelaTextBoxWithImageButton class.
        /// </summary>
        public AccelaTextBoxWithImageButton()
        {
            _imageButton = new ImageButton();
            _imageButton.Click += new ImageClickEventHandler(ButtonClick);
            _imageButton.AlternateText = LabelConvertUtil.GetGlobalTextByKey("aca_imagebutton_icontext");
            _imageButton.CausesValidation = false;
            this.Controls.Add(new LiteralControl(" "));
            this.Controls.Add(_imageButton);
        }
        #endregion

        #region Properties

        /// <summary>
        /// Image button Click Event
        /// </summary>
        public event EventHandler OnImageClick;
        
        /// <summary>
        /// Gets or sets the ImageUrl of Image Button Control
        /// </summary>
        public string ImageUrl
        {
            get
            {
                return _imageButton.ImageUrl;
            }

            set
            {
                _imageButton.ImageUrl = value;
            }
        }

        /// <summary>
        /// Gets or sets the CSS of Image Button Control
        /// </summary>
        public string ImageCssClass
        {
            get
            {
                return _imageButton.CssClass;
            }

            set
            {
                _imageButton.CssClass = value;
            }
        }

        /// <summary>
        /// Gets the Client ID of Image Button Control
        /// </summary>
        public string ImageClientID
        {
            get
            {
                return _imageButton.ClientID;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether can be clicked to perform a post back to server.
        /// </summary>
        public bool ImageEnabled
        {
            get
            {
                return _imageButton.Enabled;
            }

            set
            {
                _imageButton.Enabled = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether check custom validation or not.
        /// If it is set to true, below behavior will take affect.
        /// if the text is empty, the search button is disabled.
        /// else the search button will be enable once user enter any value in text.
        /// </summary>
        public bool ImageValidationRequired
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether run client script
        /// </summary>
        public string ImageClientScript
        {
            get;
            set;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Disable current control to make it readonly.
        /// </summary>
        public override void DisableEdit()
        {
            base.DisableEdit();
            _imageButton.Enabled = false;
        }

        /// <summary>
        /// Enable current control to make it be editable.
        /// </summary>
        public override void EnableEdit()
        {
            base.EnableEdit();
            _imageButton.Enabled = true;
        }

        /// <summary>
        /// Image Button Click
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">An System.EventArgs object that contains the event data.</param>
        protected void ButtonClick(object sender, EventArgs e)
        {
            if (OnImageClick != null)
            {
                OnImageClick(sender, e);
            }
        }

        /// <summary>
        /// Override PreRender
        /// </summary>
        /// <param name="e">An System.EventArgs object that contains the event data.</param>
        protected override void OnPreRender(EventArgs e)
        {
            _imageButton.ID = ID + "_searchButton";
            base.OnPreRender(e);
        }

        /// <summary>
        /// Override OnInitial
        /// </summary>
        /// <param name="e">An System.EventArgs object that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            //when Control ReadOnly, set Image Button ReadOnly
            if (this.ReadOnly)
            {
                _imageButton.Enabled = false;
            }

            //when ImageButton ReadOnly, set Image Grayed
            if (!_imageButton.Enabled)
            {
                _imageButton.Style.Add("filter", "Gray");
                _imageButton.Style.Add("cursor", "default");
                _imageButton.Style.Add("opacity", "0.5"); // resolved firefox issue to get the image gray.
            }
            else
            {
                _imageButton.Style.Add("cursor", "auto");

                //run Client Script
                if (!string.IsNullOrEmpty(ImageClientScript))
                {
                    _imageButton.Attributes.Add("onclick", ImageClientScript);
                }

                // Validation Input Text,
                if (ImageValidationRequired && !Page.ClientScript.IsStartupScriptRegistered(Page.GetType(), "clientScript" + this.ClientID))
                {
                    string script = @"<script language=""javascript""> 
                                        var _textBox{0} = document.getElementById(""{0}"");
                                        var _imageButton{0} = document.getElementById(""{1}"");
                                        _textBox{0}.onkeyup=inputChange{0};
                                        function inputChange{0}(){
                                            var textValue;
                                            if(typeof(GetValue) != 'undefined')
                                            {
                                                textValue = GetValue(_textBox{0});
                                            }
                                            else
                                            {
                                                textValue = _textBox{0}.value;
                                            }

                                            if(textValue.length>0)
                                            {
                                                _imageButton{0}.disabled = '';
                                                _imageButton{0}.style.filter = '';
                                                _imageButton{0}.style.cursor = 'auto';
                                                _imageButton{0}.style.opacity = 1; // enabled style
                                            }
                                            else
                                            {
                                                _imageButton{0}.disabled = 'disabled';
                                                _imageButton{0}.style.filter = 'Gray';                                            
                                                _imageButton{0}.style.cursor = 'default';
                                                _imageButton{0}.style.opacity = 0.5; // disable style
                                            }
                                        };
                                        inputChange{0}();
                                </script>".Replace("{0}", this.ClientID).Replace("{1}", _imageButton.ClientID);

                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "clientScript" + this.ClientID, script.ToString());
                }
            }
        }

        #endregion
    }
}
