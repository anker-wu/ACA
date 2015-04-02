/**
 * <pre>
 * 
 *  Accela
 *  File: SessionInfo.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2008
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: SessionInfo.cs 121852 2009-02-24 07:09:47Z ACHIEVO\jack.su $.
 *  Revision History
 *  &lt;Date&gt;,		&lt;Who&gt;,			&lt;What&gt;
 * </pre>
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
namespace Accela.Web.Controls.BreadCrump
{

    public class SessionInfo
    {
        private int _LastIndex=1;
        private bool _HasFeeForm=false;
        private bool _HasLicenseList=false;
        private bool _HasFeeEstimate = false;
        private BreadcrumpType _BreadCrumpType;
        private PermitType _PermitType;
        private Hashtable _Urls = new Hashtable();
        private bool _IsConvertToApp=false;
        private bool _IsFirstLoadFeeForm = true;
        private bool _CheckFeeFormVisible = false;
        private bool _IsHideApplicationForm;
        /// <summary>
        /// the last index user click
        /// </summary>
        public int LastIndex
        {
            get { return _LastIndex; }
            set { _LastIndex = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool HasFeeForm
        {
            get { return _HasFeeForm; }
            set { _HasFeeForm = value; }
        }

        public bool HasLicenseList
        {
            get { return _HasLicenseList; }
            set { _HasLicenseList = value; }
        }

        public bool HasFeeEstimate
        {
            get { return _HasFeeEstimate; }
            set { _HasFeeEstimate = value; }
        }

        public BreadcrumpType BreadCrumpType
        {
            get { return _BreadCrumpType; }
            set { _BreadCrumpType = value; }
        }

        public PermitType PermitType
        {
            get { return _PermitType; }
            set { _PermitType = value; }
        }

        public Hashtable Urls
        {
            get { return _Urls; }
            set { _Urls = value; }
        }
        /// <summary>
        /// if  IsConvertToApp=Y ,that meams Obtain Fee Estimate
        /// </summary>
        public bool IsConvertToApp
        {
            get { return _IsConvertToApp; }
            set { _IsConvertToApp = value; }
        }

        public bool IsFirstLoadFeeForm
        {
            get { return _IsFirstLoadFeeForm; }
            set { _IsFirstLoadFeeForm = value; }
        }
        /// <summary>
        /// if first set lastIndex to 1,set CheckFeeFormVisible to true
        /// </summary>
        public bool CheckFeeFormVisible
        {
            get { return _CheckFeeFormVisible; }
            set { _CheckFeeFormVisible = value; }
        }

        /// <summary>
        /// if configurated only fee estimated,hide application form
        /// </summary>
        public bool IsHideApplicationForm
        {
            get { return _IsHideApplicationForm; }
            set { _IsHideApplicationForm = value; }
        }

    }
}
