﻿using Kaiyuanshe.OpenHackathon.Server.Storage.Entities;
using Kaiyuanshe.OpenHackathon.Server.Storage.Tables;
using Microsoft.WindowsAzure.Storage.Table;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Kaiyuanshe.OpenHackathon.ServerTests.Storage
{
    public class AwardAssignmentTableTests
    {
        #region ListByAwardAsync
        [Test]
        public async Task ListByAwardAsync()
        {
            string hackathonName = "hack";
            string awardId = "aid";

            var awardTable = new Mock<AwardAssignmentTable> { };
            await awardTable.Object.ListByAwardAsync(hackathonName, awardId, default);

            awardTable.Verify(t => t.ExecuteQuerySegmentedAsync(
                It.Is<TableQuery<AwardAssignmentEntity>>(q => q.FilterString == "(PartitionKey eq 'hack') and (AwardId eq 'aid')"),
                It.IsAny<Action<TableQuerySegment<AwardAssignmentEntity>>>(),
                default), Times.Once);
            awardTable.VerifyNoOtherCalls();
        }
        #endregion
    }
}
