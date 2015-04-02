/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: TestAgencyProfWS.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2007-2010
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: TestAgencyProfWS.cs 178496 2010-08-10 03:31:58Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;,		&lt;Who&gt;,			&lt;What&gt;
 *  12/19/2007     		troy.yang				Initial.
 * </pre>
 */

using System;
using System.Collections.Generic;
using System.Text;

using Accela.ACA.Common;
using Accela.ACA.WSProxy;
using Accela.ACA.WSProxy.WSModel;
using NUnit.Framework;
using Accela.Test.Lib;


namespace Accela.ACA.Test.WebService
{
    [TestFixture()]
    public class TestAgencyProfWS : TestBase
    {
        private AgencyProfWebServiceService _unitUnderTest;

        [SetUp()]
        public void SetUp()
        {
            _unitUnderTest = WSFactory.Instance.GetWebService<AgencyProfWebServiceService>();
        }

        [TearDown()]
        public void TearDown()
        {
            _unitUnderTest = null;
        }

        [Test()]
        public void test()
        {
            String[] agencys = new String[2];
            agencys[0] = AgencyCode;
            agencys[1] = "11";
            //_unitUnderTest.getAgencyLogos("xxx", agencys, "ADMIN");
            
        }

        [Test()]
        public void test1()
        {
  
            //_unitUnderTest.getAllAgencyLogos("xxx");

        }

    }
}
