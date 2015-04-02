/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: TestInspectionTyepBll.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2007-2010
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: TestInsepctionTypeBll.cs 183331 2010-10-29 07:40:02Z ACHIEVO\xinter.peng $.
 *  Revision History
 *  &lt;Date&gt;,		&lt;Who&gt;,			&lt;What&gt;
 *  05/07/2007     		troy.yang				Initial.
 * </pre>
 */
using System;

using Accela.ACA.Common;

using NUnit.Framework;

using Accela.ACA.BLL.Inspection;
using Accela.ACA.WSProxy;
using Accela.Test.Lib;

namespace Accela.ACA.Test.BLL
{
    [TestFixture()]
    public class TestInsepctionTypeBll : TestBase
    {
        private IInspectionTypeBll _insepctionTypeBll;

        [SetUp()]
        public void SetUp()
        {
            _insepctionTypeBll = (IInspectionTypeBll)ObjectFactory.GetObject(typeof(IInspectionTypeBll));
        }

        [Test()]
        public void TestgetInspectionTypesByCapType()
        {
            CapTypeModel capTypeModel = new CapTypeModel();
            capTypeModel.serviceProviderCode = "xxx";
            capTypeModel.group = "Building";
            capTypeModel.type = "000";
            capTypeModel.subType = "000";
            capTypeModel.category = "000";

            QueryFormat queryFormat = null;
            CapIDModel capId = null;
            string callerID = "troy";

            InspectionTypeModel[] inspectionTypes =
                _insepctionTypeBll.GetInspectionTypesByCapType("InspectionTypeModel", capTypeModel, capId, queryFormat, callerID);

            Assert.IsNotNull(inspectionTypes);
            Assert.AreEqual(8, inspectionTypes.Length);
        }
    }
}