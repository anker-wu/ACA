#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: TrustAccountDetail.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2010-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: TrustAccountDetail.aspx.cs 172805 2010-05-15 07:04:32Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Web;
using Accela.ACA.BLL.Account;
using Accela.ACA.BLL.APO;
using Accela.ACA.Common;
using Accela.ACA.Web.Common;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.TrustAccount
{
    /// <summary>
    /// This class provide the ability to operation TrustAccountDetail. 
    /// </summary>
    public partial class TrustAccountDetail : BasePage
    {
        #region Properties

        /// <summary>
        /// Gets account id by request collection.
        /// </summary>
        private string AccountID
        {
            get
            {
                return Request.QueryString["accountID"];
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Raises the page load event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                btnDeposit.ToolTip = LabelUtil.GetGlobalTextByKey("per_trustaccountdetail_deposit");
                InitUI();
            }
        }

        /// <summary>
        /// click "Deposit" button
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">An System.EventArgs object that contains the event data.</param>
        protected void DepositButton_OnClick(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(AccountID))
            {
                Response.Redirect(string.Format("../Account/TrustAccountDeposit.aspx?accountID={0}", HttpUtility.UrlEncode(AccountID)));
             }
         }

        /// <summary>
        /// Display trust detail info, include trust account detail, trust account associate address, parcel, people and transaction.
        /// </summary>
        private void InitUI()
        {
            if (AppSession.IsAdmin)
            {
                DisplayEmptyRecordList();
            }
            else if (!string.IsNullOrEmpty(AccountID))
            {
                //1. Get Trust account model by account id.
                ITrustAccountBll trustAccountBll = (ITrustAccountBll)ObjectFactory.GetObject(typeof(ITrustAccountBll));
                TrustAccountModel trustAccount = trustAccountBll.GetTrustAccountAndTransactionByAccountID(AccountID);

                if (trustAccount != null)
                {
                    //2. Get current trust account seqence number. 
                    string accountSeqNumber = trustAccount.acctSeq.HasValue ? trustAccount.acctSeq.ToString() : string.Empty;

                    //3. Display trust account detail information.
                    trustAcctDetail.Display(trustAccount);

                    //4. Display trust account associate address information.
                    RefAddressModel[] refAddresses = GetAssociatedAddress(accountSeqNumber);
                    refAddressList.BindList(refAddresses);

                    //5. Display trust  account associated parcel information.
                    ParcelInfoModel[] parcelInfos = GetAssociatedParcelInfo(accountSeqNumber);
                    refParcelList.BindList(parcelInfos);

                    //6. Display trust  account associated people information.
                    TrustAccountPeopleModel[] trustAccountPeople = trustAccountBll.GetTrustAccountPeopleListByAccountNumber(accountSeqNumber);
                    peopleList.BindList(trustAccountPeople);

                    //7. Display trust account associated transaction.
                    transactionsList.BindList(trustAccount);
                }
                else
                {
                    DisplayEmptyRecordList();
                }
            }
        }

        /// <summary>
        /// Display empty record list
        /// </summary>
        private void DisplayEmptyRecordList()
        {
            refParcelList.BindList(null);
            refAddressList.BindList(null);
            peopleList.BindList(null);
            transactionsList.BindList(null);
        }

        /// <summary>
        /// Get trust account associated address.
        /// </summary>
        /// <param name="accountSeqNumber">account sequence number</param>
        /// <returns>RefAddressModel array.</returns>
        private RefAddressModel[] GetAssociatedAddress(string accountSeqNumber)
        {
            IRefAddressBll refAddressBll = (IRefAddressBll)ObjectFactory.GetObject(typeof(IRefAddressBll));
            RefAddressModel[] refAddresses = refAddressBll.GetAddressListByTrustAccount(accountSeqNumber);

            return refAddresses;
        }

        /// <summary>
        /// Get trust  account associated parcel.
        /// </summary>
        /// <param name="accountSeqNumber">account sequence number</param>
        /// <returns>ParcelInfoModel array.</returns>
        private ParcelInfoModel[] GetAssociatedParcelInfo(string accountSeqNumber)
        {
            IParcelBll parcelBll = (IParcelBll)ObjectFactory.GetObject(typeof(IParcelBll));
            ParcelInfoModel[] parcelInfos = parcelBll.GetParcelListByTrustAccount(accountSeqNumber);

            return parcelInfos;
        }

        #endregion Methods
    }
}
