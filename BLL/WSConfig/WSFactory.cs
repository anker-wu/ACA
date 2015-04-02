/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: WSFactory.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2007-2008
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: WSFactory.cs 121852 2009-02-24 07:09:47Z ACHIEVO\jack.su $.
 *  Revision History
 *  &lt;Date&gt;,		&lt;Who&gt;,			&lt;What&gt;
 *  08/15/2007           Jackie.Yu              Initial.
 * </pre>
 */

using System.Net;

using Accela.ACA.Common.Config;
using Accela.ACA.WSProxy;

namespace Accela.ACA.BLL
{
    /// <summary>
    /// Web Service Factory to ensure that all of web service instance is created by this factory.
    /// WSFactory is singleton class.
    /// </summary>
    public sealed class WSFactory
    {
        #region Private Variables

        private AppSpecificInfoWebServiceService _appSpecificInfoService = null;
        private BizDomainWebServiceService _bizDomainService = null;
        private CalendarWebServiceService _calendarService = null;
        private CapTypeWebServiceService _capTypeService = null;
        private CapWebServiceService _capService = null;
        private CashierWebServiceService _cashierService = null;
        private ContractorLicenseWebServiceService _contractorLicenseService = null;
        private EDMSDocumentWebServiceService _eDMSDocumentService = null;
        private FeeWebServiceService _feeService = null;
        private GISWebServiceService _GISService = null;
        private InspectionTypeWebServiceService _inspectionTypeService = null;
        private InspectionWebServiceService _inspectionService = null;
        private LicenseWebServiceService _licenseService = null;
        private OnlinePaymentWebServiceService _onlinePaymentService = null;
        private PaymentWebServiceService _paymentService = null;
        private PeopleWebServiceService _peopleService = null;
        private PlanServiceService _planService = null;
        private PublicUserWebServiceService _publicUserService = null;
        private RefAddressWebServiceService _refAddressService = null;
        private ServerConstantWebServiceService _serverConstantService = null;
        private ServiceProviderWebServiceService _serviceProviderService = null;
        private SmartChoiceGroupWebServiceService _smartChoiceGroupService = null;
        private SSOWebServiceService _SSOService = null;
        private TimeZoneWebServiceService _timeZoneService = null;
        private WorkflowWebServiceService _workflowService = null;
        private GenericViewWebServiceService _viewService = null;
        private TrustAccountWebServiceService _trustAccountService = null;
        private GUITextWebServiceService _guiTextService = null;
        private ReportWebServiceService _reportService = null;
        private OwnerWebServiceService _ownerService = null;
        private ParcelWebServiceService _parcelService = null;
        private TemplateWebServiceService _templateService = null;
        private AcaAdminTreeWebServiceService _acaAdminTreeWebService = null;
        #endregion

        #region Constructors

        /// <summary>
        /// private constructor avoid to be instance by caller.
        /// </summary>
        private WSFactory()
        {
            // Set the maximum idle time of a ServicePoint instance to 15 seconds.
            // After the idle time expires, the ServicePoint object is eligible for
            // garbage collection and cannot be used by the ServicePointManager object.
            // Notice that MaxServicePointIdleTime value must less than the tomcat connectionTimeout value 
            // to make sure that client have released the connection resource before Tomcat release it.
            // Tomcat connectionTimeout value can be configed in jbossweb-tomcat55.sar\server.xml, and the default value is 20 seconds. 
            // Make sure the idle timeout(MaxServicePointIdleTime) on the client side is less than that on the server side(in jbossweb-tomcat55.sar\server.xml)    
            ServicePointManager.MaxServicePointIdleTime = 15000;
        }

        #endregion Constructors

        #region Public Propertiess

        /// <summary>
        /// singleton pattern.
        /// </summary>
        public static readonly WSFactory Instance = new WSFactory();

        #endregion Public Propertiess

        #region Public Methods

        /// <summary>
        /// singleton pattern implement
        /// </summary>
        /// <returns>The instance of AppSpecificInfoWebServiceService.</returns>
        public AppSpecificInfoWebServiceService GetAppSpecificInfoService()
        {
            if (_appSpecificInfoService == null)
            {
                lock (typeof (AppSpecificInfoWebServiceService))
                {
                    if (_appSpecificInfoService == null)
                    {
                        _appSpecificInfoService = new AppSpecificInfoWebServiceService();
                        WebServiceParameter p = WebServiceConfig.GetWebServiceParameter(typeof (AppSpecificInfoWebServiceService));
                        _appSpecificInfoService.Url = p.Url;
                        _appSpecificInfoService.Timeout = p.Timeout;
                    }
                }
            }
            return _appSpecificInfoService;
        }

        /// <summary>
        /// singleton pattern implement
        /// </summary>
        /// <returns>The instance of BizDomainWebServiceService.</returns>
        public BizDomainWebServiceService GetBizDomainService()
        {
            if (_bizDomainService == null)
            {
                lock (typeof (BizDomainWebServiceService))
                {
                    if (_bizDomainService == null)
                    {
                        _bizDomainService = new BizDomainWebServiceService();
                        WebServiceParameter p = WebServiceConfig.GetWebServiceParameter(typeof (BizDomainWebServiceService));
                        _bizDomainService.Url = p.Url;
                        _bizDomainService.Timeout = p.Timeout;
                    }
                }
            }
            return _bizDomainService;
        }

        /// <summary>
        /// singleton pattern implement
        /// </summary>
        /// <returns>The instance of CalendarWebServiceService.</returns>
        public CalendarWebServiceService GetCalendarService()
        {
            if (_calendarService == null)
            {
                lock (typeof (CalendarWebServiceService))
                {
                    if (_calendarService == null)
                    {
                        _calendarService = new CalendarWebServiceService();
                        WebServiceParameter p = WebServiceConfig.GetWebServiceParameter(typeof (CalendarWebServiceService));
                        _calendarService.Url = p.Url;
                        _calendarService.Timeout = p.Timeout;
                    }
                }
            }
            return _calendarService;
        }

        /// <summary>
        /// singleton pattern implement
        /// </summary>
        /// <returns>The instance of CapTypeWebServiceService.</returns>
        public CapTypeWebServiceService GetCapTypeService()
        {
            if (_capTypeService == null)
            {
                lock (typeof (CapTypeWebServiceService))
                {
                    if (_capTypeService == null)
                    {
                        _capTypeService = new CapTypeWebServiceService();
                        WebServiceParameter p = WebServiceConfig.GetWebServiceParameter(typeof (CapTypeWebServiceService));
                        _capTypeService.Url = p.Url;
                        _capTypeService.Timeout = p.Timeout;
                    }
                }
            }
            return _capTypeService;
        }

        /// <summary>
        /// singleton pattern implement
        /// </summary>
        /// <returns>The instance of CapWebServiceService.</returns>
        public CapWebServiceService GetCapService()
        {
            if (_capService == null)
            {
                lock (typeof (CapWebServiceService))
                {
                    if (_capService == null)
                    {
                        _capService = new CapWebServiceService();
                        WebServiceParameter p = WebServiceConfig.GetWebServiceParameter(typeof (CapWebServiceService));
                        _capService.Url = p.Url;
                        _capService.Timeout = p.Timeout;
                    }
                }
            }
            return _capService;
        }

        /// <summary>
        /// singleton pattern implement
        /// </summary>
        /// <returns>The instance of CashierWebServiceService.</returns>
        public CashierWebServiceService GetCashierService()
        {
            if (_cashierService == null)
            {
                lock (typeof (CashierWebServiceService))
                {
                    if (_cashierService == null)
                    {
                        _cashierService = new CashierWebServiceService();
                        WebServiceParameter p = WebServiceConfig.GetWebServiceParameter(typeof (CashierWebServiceService));
                        _cashierService.Url = p.Url;
                        _cashierService.Timeout = p.Timeout;
                    }
                }
            }
            return _cashierService;
        }

        /// <summary>
        /// singleton pattern implement
        /// </summary>
        /// <returns>The instance of ContractorLicenseWebServiceService.</returns>
        public ContractorLicenseWebServiceService GetContractorLicenseService()
        {
            if (_contractorLicenseService == null)
            {
                lock (typeof (ContractorLicenseWebServiceService))
                {
                    if (_contractorLicenseService == null)
                    {
                        _contractorLicenseService = new ContractorLicenseWebServiceService();
                        WebServiceParameter p = WebServiceConfig.GetWebServiceParameter(typeof (ContractorLicenseWebServiceService));
                        _contractorLicenseService.Url = p.Url;
                        _contractorLicenseService.Timeout = p.Timeout;
                    }
                }
            }
            return _contractorLicenseService;
        }

        /// <summary>
        /// singleton pattern implement
        /// </summary>
        /// <returns>The instance of EDMSDocumentWebServiceService.</returns>
        public EDMSDocumentWebServiceService GetEDMSDocumentService()
        {
            if (_eDMSDocumentService == null)
            {
                lock (typeof (EDMSDocumentWebServiceService))
                {
                    if (_eDMSDocumentService == null)
                    {
                        _eDMSDocumentService = new EDMSDocumentWebServiceService();
                        WebServiceParameter p = WebServiceConfig.GetWebServiceParameter(typeof (EDMSDocumentWebServiceService));
                        _eDMSDocumentService.Url = p.Url;
                        _eDMSDocumentService.Timeout = p.Timeout;
                    }
                }
            }
            return _eDMSDocumentService;
        }

        /// <summary>
        /// singleton pattern implement
        /// </summary>
        /// <returns>The instance of FeeWebServiceService.</returns>
        public FeeWebServiceService GetFeeService()
        {
            if (_feeService == null)
            {
                lock (typeof (FeeWebServiceService))
                {
                    if (_feeService == null)
                    {
                        _feeService = new FeeWebServiceService();
                        WebServiceParameter p = WebServiceConfig.GetWebServiceParameter(typeof (FeeWebServiceService));
                        _feeService.Url = p.Url;
                        _feeService.Timeout = p.Timeout;
                    }
                }
            }
            return _feeService;
        }

        /// <summary>
        /// singleton pattern implement
        /// </summary>
        /// <returns>The instance of GISWebServiceService.</returns>
        public GISWebServiceService GetGISService()
        {
            if (_GISService == null)
            {
                lock (typeof (GISWebServiceService))
                {
                    if (_GISService == null)
                    {
                        _GISService = new GISWebServiceService();
                        WebServiceParameter p = WebServiceConfig.GetWebServiceParameter(typeof (GISWebServiceService));
                        _GISService.Url = p.Url;
                        _GISService.Timeout = p.Timeout;
                    }
                }
            }
            return _GISService;
        }

        /// <summary>
        /// singleton pattern implement
        /// </summary>
        /// <returns>The instance of InspectionTypeWebServiceService.</returns>
        public InspectionTypeWebServiceService GetInspectionTypeService()
        {
            if (_inspectionTypeService == null)
            {
                lock (typeof (InspectionTypeWebServiceService))
                {
                    if (_inspectionTypeService == null)
                    {
                        _inspectionTypeService = new InspectionTypeWebServiceService();
                        WebServiceParameter p = WebServiceConfig.GetWebServiceParameter(typeof (InspectionTypeWebServiceService));
                        _inspectionTypeService.Url = p.Url;
                        _inspectionTypeService.Timeout = p.Timeout;
                    }
                }
            }
            return _inspectionTypeService;
        }

        /// <summary>
        /// singleton pattern implement
        /// </summary>
        /// <returns>The instance of InspectionWebServiceService.</returns>
        public InspectionWebServiceService GetInspectionService()
        {
            if (_inspectionService == null)
            {
                lock (typeof (InspectionWebServiceService))
                {
                    if (_inspectionService == null)
                    {
                        _inspectionService = new InspectionWebServiceService();
                        WebServiceParameter p = WebServiceConfig.GetWebServiceParameter(typeof (InspectionWebServiceService));
                        _inspectionService.Url = p.Url;
                        _inspectionService.Timeout = p.Timeout;
                    }
                }
            }
            return _inspectionService;
        }

        /// <summary>
        /// singleton pattern implement
        /// </summary>
        /// <returns>The instance of LicenseWebServiceService.</returns>
        public LicenseWebServiceService GetLicenseService()
        {
            if (_licenseService == null)
            {
                lock (typeof (LicenseWebServiceService))
                {
                    if (_licenseService == null)
                    {
                        _licenseService = new LicenseWebServiceService();
                        WebServiceParameter p = WebServiceConfig.GetWebServiceParameter(typeof (LicenseWebServiceService));
                        _licenseService.Url = p.Url;
                        _licenseService.Timeout = p.Timeout;
                    }
                }
            }
            return _licenseService;
        }

        /// <summary>
        /// singleton pattern implement
        /// </summary>
        /// <returns>The instance of OnlinePaymentWebServiceService.</returns>
        public OnlinePaymentWebServiceService GetOnlinePaymentService()
        {
            if (_onlinePaymentService == null)
            {
                lock (typeof (OnlinePaymentWebServiceService))
                {
                    if (_onlinePaymentService == null)
                    {
                        _onlinePaymentService = new OnlinePaymentWebServiceService();
                        WebServiceParameter p = WebServiceConfig.GetWebServiceParameter(typeof (OnlinePaymentWebServiceService));
                        _onlinePaymentService.Url = p.Url;
                        _onlinePaymentService.Timeout = p.Timeout;
                    }
                }
            }
            return _onlinePaymentService;
        }

        /// <summary>
        /// singleton pattern implement
        /// </summary>
        /// <returns>The instance of PaymentWebServiceService.</returns>
        public PaymentWebServiceService GetPaymentService()
        {
            if (_paymentService == null)
            {
                lock (typeof (PaymentWebServiceService))
                {
                    if (_paymentService == null)
                    {
                        _paymentService = new PaymentWebServiceService();
                        WebServiceParameter p = WebServiceConfig.GetWebServiceParameter(typeof (PaymentWebServiceService));
                        _paymentService.Url = p.Url;
                        _paymentService.Timeout = p.Timeout;
                    }
                }
            }
            return _paymentService;
        }

        /// <summary>
        /// singleton pattern implement
        /// </summary>
        /// <returns>The instance of PeopleWebServiceService.</returns>
        public PeopleWebServiceService GetPeopleService()
        {
            if (_peopleService == null)
            {
                lock (typeof (PeopleWebServiceService))
                {
                    if (_peopleService == null)
                    {
                        _peopleService = new PeopleWebServiceService();
                        WebServiceParameter p = WebServiceConfig.GetWebServiceParameter(typeof (PeopleWebServiceService));
                        _peopleService.Url = p.Url;
                        _peopleService.Timeout = p.Timeout;
                    }
                }
            }
            return _peopleService;
        }

        /// <summary>
        /// singleton pattern implement
        /// </summary>
        /// <returns>The instance of PlanServiceService.</returns>
        public PlanServiceService GetPlanService()
        {
            if (_planService == null)
            {
                lock (typeof (PlanServiceService))
                {
                    if (_planService == null)
                    {
                        _planService = new PlanServiceService();
                        WebServiceParameter p = WebServiceConfig.GetWebServiceParameter(typeof (PlanServiceService));
                        _planService.Url = p.Url;
                        _planService.Timeout = p.Timeout;
                    }
                }
            }
            return _planService;
        }

        /// <summary>
        /// singleton pattern implement
        /// </summary>
        /// <returns>The instance of PublicUserWebServiceService.</returns>
        public PublicUserWebServiceService GetPublicUserService()
        {
            if (_publicUserService == null)
            {
                lock (typeof (PublicUserWebServiceService))
                {
                    if (_publicUserService == null)
                    {
                        _publicUserService = new PublicUserWebServiceService();
                        WebServiceParameter p = WebServiceConfig.GetWebServiceParameter(typeof (PublicUserWebServiceService));
                        _publicUserService.Url = p.Url;
                        _publicUserService.Timeout = p.Timeout;
                    }
                }
            }
            return _publicUserService;
        }

        /// <summary>
        /// singleton pattern implement
        /// </summary>
        /// <returns>The instance of RefAddressWebServiceService.</returns>
        public RefAddressWebServiceService GetRefAddressService()
        {
            if (_refAddressService == null)
            {
                lock (typeof (RefAddressWebServiceService))
                {
                    if (_refAddressService == null)
                    {
                        _refAddressService = new RefAddressWebServiceService();
                        WebServiceParameter p = WebServiceConfig.GetWebServiceParameter(typeof (RefAddressWebServiceService));
                        _refAddressService.Url = p.Url;
                        _refAddressService.Timeout = p.Timeout;
                    }
                }
            }
            return _refAddressService;
        }

        /// <summary>
        /// singleton pattern implement
        /// </summary>
        /// <returns>The instance of ServerConstantWebServiceService.</returns>
        public ServerConstantWebServiceService GetServerConstantService()
        {
            if (_serverConstantService == null)
            {
                lock (typeof (ServerConstantWebServiceService))
                {
                    if (_serverConstantService == null)
                    {
                        _serverConstantService = new ServerConstantWebServiceService();
                        WebServiceParameter p = WebServiceConfig.GetWebServiceParameter(typeof (ServerConstantWebServiceService));
                        _serverConstantService.Url = p.Url;
                        _serverConstantService.Timeout = p.Timeout;
                    }
                }
            }

            return _serverConstantService;
        }

        /// <summary>
        /// singleton pattern implement
        /// </summary>
        /// <returns>The instance of ServiceProviderWebServiceService.</returns>
        public ServiceProviderWebServiceService GetServiceProviderService()
        {
            if (_serviceProviderService == null)
            {
                lock (typeof (ServiceProviderWebServiceService))
                {
                    if (_serviceProviderService == null)
                    {
                        _serviceProviderService = new ServiceProviderWebServiceService();
                        WebServiceParameter p = WebServiceConfig.GetWebServiceParameter(typeof (ServiceProviderWebServiceService));
                        _serviceProviderService.Url = p.Url;
                        _serviceProviderService.Timeout = p.Timeout;
                    }
                }
            }
            return _serviceProviderService;
        }

        /// <summary>
        /// singleton pattern implement
        /// </summary>
        /// <returns>The instance of SmartChoiceGroupWebServiceService.</returns>
        public SmartChoiceGroupWebServiceService GetSmartChoiceGroupService()
        {
            if (_smartChoiceGroupService == null)
            {
                lock (typeof (SmartChoiceGroupWebServiceService))
                {
                    if (_smartChoiceGroupService == null)
                    {
                        _smartChoiceGroupService = new SmartChoiceGroupWebServiceService();
                        WebServiceParameter p = WebServiceConfig.GetWebServiceParameter(typeof (SmartChoiceGroupWebServiceService));
                        _smartChoiceGroupService.Url = p.Url;
                        _smartChoiceGroupService.Timeout = p.Timeout;
                    }
                }
            }
            return _smartChoiceGroupService;
        }

        /// <summary>
        /// singleton pattern implement
        /// </summary>
        /// <returns>The instance of SSOWebServiceService.</returns>
        public SSOWebServiceService GetSSOService()
        {
            if (_SSOService == null)
            {
                lock (typeof (SSOWebServiceService))
                {
                    if (_SSOService == null)
                    {
                        _SSOService = new SSOWebServiceService();
                        WebServiceParameter p = WebServiceConfig.GetWebServiceParameter(typeof (SSOWebServiceService));
                        _SSOService.Url = p.Url;
                        _SSOService.Timeout = p.Timeout;
                    }
                }
            }

            return _SSOService;
        }

        /// <summary>
        /// singleton pattern implement
        /// </summary>
        /// <returns>The instance of TimeZoneWebServiceService.</returns>
        public TimeZoneWebServiceService GetTimeZoneService()
        {
            if (_timeZoneService == null)
            {
                lock (typeof (TimeZoneWebServiceService))
                {
                    if (_timeZoneService == null)
                    {
                        _timeZoneService = new TimeZoneWebServiceService();
                        WebServiceParameter p = WebServiceConfig.GetWebServiceParameter(typeof (TimeZoneWebServiceService));
                        _timeZoneService.Url = p.Url;
                        _timeZoneService.Timeout = p.Timeout;
                    }
                }
            }

            return _timeZoneService;
        }

        /// <summary>
        /// singleton pattern implement
        /// </summary>
        /// <returns>The instance of WorkflowWebServiceService.</returns>
        public WorkflowWebServiceService GetWorkflowService()
        {
            if (_workflowService == null)
            {
                lock (typeof (WorkflowWebServiceService))
                {
                    if (_workflowService == null)
                    {
                        _workflowService = new WorkflowWebServiceService();
                        WebServiceParameter p = WebServiceConfig.GetWebServiceParameter(typeof (WorkflowWebServiceService));
                        _workflowService.Url = p.Url;
                        _workflowService.Timeout = p.Timeout;
                    }
                }
            }

            return _workflowService;
        }

        /// <summary>
        /// singleton pattern implement
        /// </summary>
        /// <returns>The instance of GenericViewWebServiceService.</returns>
        public GenericViewWebServiceService GetGenericViewService()
        {
            if (_viewService == null)
            {
                lock (typeof(GenericViewWebServiceService))
                {
                    if (_viewService == null)
                    {
                        _viewService = new GenericViewWebServiceService();
                        WebServiceParameter p = WebServiceConfig.GetWebServiceParameter(typeof(GenericViewWebServiceService));
                        _viewService.Url = p.Url;
                        _viewService.Timeout = p.Timeout;
                    }
                }
            }

            return _viewService;
        }

        /// <summary>
        /// singleton pattern implement
        /// </summary>
        /// <returns>The instance of AppSpecificInfoWebServiceService.</returns>
        public TrustAccountWebServiceService GetTrustAccountService()
        {
            if (_trustAccountService == null)
            {
                lock (typeof(TrustAccountWebServiceService))
                {
                    if (_trustAccountService == null)
                    {
                        _trustAccountService = new TrustAccountWebServiceService();
                        WebServiceParameter p = WebServiceConfig.GetWebServiceParameter(typeof(TrustAccountWebServiceService));
                        _trustAccountService.Url = p.Url;
                        _trustAccountService.Timeout = p.Timeout;
                    }
                }
            }
            return _trustAccountService;
        }

        /// <summary>
        /// singleton pattern implement
        /// </summary>
        /// <returns>The instance of AppSpecificInfoWebServiceService.</returns>
        public ReportWebServiceService GetReportService()
        {
            if (_reportService == null)
            {
                lock (typeof(ReportWebServiceService))
                {
                    if (_reportService == null)
                    {
                        _reportService = new ReportWebServiceService();
                        WebServiceParameter p = WebServiceConfig.GetWebServiceParameter(typeof(ReportWebServiceService));
                        _reportService.Url = p.Url;
                        _reportService.Timeout = p.Timeout;
                    }
                }
            }
            return _reportService;
        }
        
        /// <summary>
        /// singleton pattern implement
        /// </summary>
        /// <returns>The instance of GUITextWebServiceService.</returns>
        public GUITextWebServiceService GetGUITextService()
        {
            if (_guiTextService == null)
            {
                lock (typeof(GUITextWebServiceService))
                {
                    if (_guiTextService == null)
                    {
                        _guiTextService = new GUITextWebServiceService();
                        WebServiceParameter p = WebServiceConfig.GetWebServiceParameter(typeof(GUITextWebServiceService));
                        _guiTextService.Url = p.Url;
                        _guiTextService.Timeout = p.Timeout;
                    }
                }
            }
            return _guiTextService;
        }

        /// <summary>
        /// singleton pattern implement
        /// </summary>
        /// <returns>The instance of OwnerWebServiceService.</returns>
        public OwnerWebServiceService GetOwnerService()
        {
            if (_ownerService == null)
            {
                lock (typeof(OwnerWebServiceService))
                {
                    if (_ownerService == null)
                    {
                        _ownerService = new OwnerWebServiceService();
                        WebServiceParameter p = WebServiceConfig.GetWebServiceParameter(typeof(OwnerWebServiceService));
                        _ownerService.Url = p.Url;
                        _ownerService.Timeout = p.Timeout;
                    }
                }
            }
            return _ownerService;
        }

        /// <summary>
        /// singleton pattern implement
        /// </summary>
        /// <returns>The instance of ParcelWebServiceService.</returns>
        public ParcelWebServiceService GetParcelService()
        {
            if (_parcelService == null)
            {
                lock (typeof(ParcelWebServiceService))
                {
                    if (_parcelService == null)
                    {
                        _parcelService = new ParcelWebServiceService();
                        WebServiceParameter p = WebServiceConfig.GetWebServiceParameter(typeof(ParcelWebServiceService));
                        _parcelService.Url = p.Url;
                        _parcelService.Timeout = p.Timeout;
                    }
                }
            }
            return _parcelService;
        }

        /// <summary>
        /// singleton pattern implement
        /// </summary>
        /// <returns>The instance of TemplateWebServiceService.</returns>
        public TemplateWebServiceService GetTemplateService()
        {
            if (_templateService == null)
            {
                lock (typeof(TemplateWebServiceService))
                {
                    if (_templateService == null)
                    {
                        _templateService = new TemplateWebServiceService();
                        WebServiceParameter p = WebServiceConfig.GetWebServiceParameter(typeof(TemplateWebServiceService));
                        _templateService.Url = p.Url;
                        _templateService.Timeout = p.Timeout;
                    }
                }
            }
            return _templateService;
        }
        /// <summary>
        /// singleton pattern implement
        /// </summary>
        /// <returns>The instance of AcaAdminTreeWebServiceService.</returns>
        public AcaAdminTreeWebServiceService GetAcaAdminTreeService()
        {
            if (_acaAdminTreeWebService == null)
            {
                lock (typeof(AcaAdminTreeWebServiceService))
                {
                    if (_acaAdminTreeWebService == null)
                    {
                        _acaAdminTreeWebService = new AcaAdminTreeWebServiceService();
                        WebServiceParameter p = WebServiceConfig.GetWebServiceParameter(typeof(AcaAdminTreeWebServiceService));
                        _acaAdminTreeWebService.Url = p.Url;
                        _acaAdminTreeWebService.Timeout = p.Timeout;
                    }
                }
            }
            return _acaAdminTreeWebService;
        }

        #endregion Public Methods
    }
}
