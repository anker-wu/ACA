/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: TestCapWS.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2007-2010
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: TestExpressionWS.cs 178496 2010-08-10 03:31:58Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;,		&lt;Who&gt;,			&lt;What&gt;
 * </pre>
 */
using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

using Accela.ACA.Common;
using Accela.ACA.WSProxy;
using Accela.ACA.WSProxy.WSModel;
using Accela.Test.Lib;

namespace Accela.ACA.Test.WebService
{
    [TestFixture()]
    public class TestExpressionWS : TestBase
    {
        private ExpressionWebServiceService _unitUnderTest;

        [SetUp()]
        public void SetUp()
        {
            _unitUnderTest = WSFactory.Instance.GetWebService<ExpressionWebServiceService>();
        }

        [TearDown()]
        public void TearDown()
        {
            _unitUnderTest = null;
        }

        [Test()]
        public void TestGetExecuteFieldListByViewID()
        {

            CapIDModel4WS capId = new CapIDModel4WS();

            CapTypeModel capType = new CapTypeModel();
            capType.serviceProviderCode = AgencyCode;
            capType.group = "Building";
            capType.type = "Anderson";
            capType.subType = "Demo";
            capType.category = "professional";

            string callerID = "ACA AutoUT";
            string viewID = "-1";
            string viewKey1 = "";
            string viewKey2 = "";

            //ExpressionFieldModel4WS[] resultArray =
            //    _unitUnderTest.getRefExecuteFieldListByViewID(capType, viewID, viewKey1, viewKey2, capId,callerID);

            //Assert.IsNotNull(resultArray);

        }

        public void TestGetExpressionParameters()
        {
            RefExpressionPK refExpressionPK = new RefExpressionPK();
            refExpressionPK.servProvCode = AgencyCode;
            refExpressionPK.expressionName = "Anderson-ASI_Lookup";

            CapIDModel4WS capID4WS = new CapIDModel4WS();
            capID4WS.customID = "07AND-00000-00001";
            capID4WS.id1 = "07AND";
            capID4WS.id2 = "00000";
            capID4WS.id3 = "00001";
            capID4WS.serviceProviderCode = AgencyCode;

            //ExpressionDTOModel4WS expressionDTO = _unitUnderTest.getExpressionParameters(refExpressionPK, capID4WS);

            //Assert.IsNotNull(expressionDTO);
        }

        public void TestRunExpression()
        {        
            RefExpressionPK refExpressionPK = new RefExpressionPK();
            refExpressionPK.expressionName = "Anderson-ASI_Lookup";

            CapIDModel4WS capID4WS = new CapIDModel4WS();
            capID4WS.customID = "07AND-00000-00001";
            capID4WS.id1 = "07AND";
            capID4WS.id2 = "00000";
            capID4WS.id3 = "00001";
            capID4WS.serviceProviderCode = AgencyCode;

            //ExpressionDTOModel4WS expressionDTO = _unitUnderTest.getExpressionParameters(refExpressionPK, capID4WS);

            //ExpressionFieldModel4WS[] inputParams4WS = expressionDTO.inputParams;

            //ExpressionFieldModel4WS[][] tableRecords4WS = null;

            //long[] portletsInPage4WS = new long[]{-1,-99999};
            //string callerID = "ACA AutoUT";

            //ExpressionDTOModel4WS resultDTO = _unitUnderTest.runExpression(refExpressionPK, inputParams4WS, tableRecords4WS, portletsInPage4WS, callerID);

            //Assert.IsNotNull(resultDTO);
        }
    }
}
