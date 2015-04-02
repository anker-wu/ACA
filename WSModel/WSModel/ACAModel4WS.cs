/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: ACAModel4WS.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2007-2009
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: ACAModel4WS.cs 130107 2009-05-11 12:23:56Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

namespace Accela.ACA.WSProxy
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://model.webservice.accela.com")]
    public partial class ACAModel4WS
    {

        private string callerIDField;

        private string capStateAfterApplyField;

        private string isACAField;

        private string moduleField;

        private string servProvCodeField;

        private string serverNameField;

        private string strActionField;

        private SysUserModel4WS suModelField;

        private string taskDispositionCommentField;

        private bool updateStatusField;

        private BizDomainModel4WS[] workflowTasksField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string callerID
        {
            get
            {
                return this.callerIDField;
            }
            set
            {
                this.callerIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string capStateAfterApply
        {
            get
            {
                return this.capStateAfterApplyField;
            }
            set
            {
                this.capStateAfterApplyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string isACA
        {
            get
            {
                return this.isACAField;
            }
            set
            {
                this.isACAField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string module
        {
            get
            {
                return this.moduleField;
            }
            set
            {
                this.moduleField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string servProvCode
        {
            get
            {
                return this.servProvCodeField;
            }
            set
            {
                this.servProvCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string serverName
        {
            get
            {
                return this.serverNameField;
            }
            set
            {
                this.serverNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string strAction
        {
            get
            {
                return this.strActionField;
            }
            set
            {
                this.strActionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public SysUserModel4WS suModel
        {
            get
            {
                return this.suModelField;
            }
            set
            {
                this.suModelField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string taskDispositionComment
        {
            get
            {
                return this.taskDispositionCommentField;
            }
            set
            {
                this.taskDispositionCommentField = value;
            }
        }

        /// <remarks/>
         [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public bool updateStatus
        {
            get
            {
                return this.updateStatusField;
            }
            set
            {
                this.updateStatusField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("workflowTasks", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public BizDomainModel4WS[] workflowTasks
        {
            get
            {
                return this.workflowTasksField;
            }
            set
            {
                this.workflowTasksField = value;
            }
        }
    }
}
