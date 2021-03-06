﻿using Kaiyuanshe.OpenHackathon.Server.Models;
using Kaiyuanshe.OpenHackathon.Server.ResponseBuilder;
using Kaiyuanshe.OpenHackathon.Server.Storage.Entities;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Kaiyuanshe.OpenHackathon.ServerTests.ResponseBuilder
{
    [TestFixture]
    public class DefaultResponseBuilderTest
    {
        [Test]
        public void BuildHackathonTest()
        {
            var entity = new HackathonEntity
            {
                PartitionKey = "pk",
                AutoApprove = true,
                CreatorId = "abc",
                Location = "loc",
                JudgeStartedAt = DateTime.UtcNow,
                Status = HackathonStatus.online,
                Enrollment = 100,
                ReadOnly = true,
            };

            var roles = new HackathonRoles
            {
                isAdmin = true,
                isEnrolled = true,
                isJudge = true,
            };

            var builder = new DefaultResponseBuilder();
            var hack = builder.BuildHackathon(entity, roles);

            Assert.IsNull(hack.tags);
            Assert.AreEqual("pk", hack.name);
            Assert.IsTrue(hack.autoApprove.Value);
            Assert.AreEqual("loc", hack.location);
            Assert.AreEqual("abc", hack.creatorId);
            Assert.IsTrue(hack.judgeStartedAt.HasValue);
            Assert.IsFalse(hack.judgeEndedAt.HasValue);
            Assert.AreEqual(100, hack.enrollment);
            Assert.IsTrue(hack.roles.isAdmin);
            Assert.IsTrue(hack.roles.isEnrolled);
            Assert.IsTrue(hack.roles.isJudge);
            Assert.IsTrue(hack.readOnly);
        }

        [Test]
        public void BuildEnrollmentTest()
        {
            EnrollmentEntity entity = new EnrollmentEntity
            {
                PartitionKey = "hack",
                RowKey = "uid",
                Status = EnrollmentStatus.approved,
                CreatedAt = DateTime.Now,
                Timestamp = DateTime.Now,
            };
            UserInfo userInfo = new UserInfo
            {
                Name = "name"
            };

            var respBuilder = new DefaultResponseBuilder();
            var enrollment = respBuilder.BuildEnrollment(entity, userInfo);

            Assert.AreEqual("hack", enrollment.hackathonName);
            Assert.AreEqual("uid", enrollment.userId);
            Assert.AreEqual(EnrollmentStatus.approved, enrollment.status);
            Assert.AreEqual(entity.CreatedAt, enrollment.createdAt);
            Assert.AreEqual(entity.Timestamp.DateTime, enrollment.updatedAt);
            Assert.AreEqual("name", enrollment.user.Name);
        }

        [Test]
        public void BuildTeamTest()
        {
            var entity = new TeamEntity
            {
                PartitionKey = "pk",
                RowKey = "rk",
                AutoApprove = false,
                CreatorId = "uid",
                Description = "desc",
                DisplayName = "dp",
                CreatedAt = DateTime.Now,
                Timestamp = DateTime.Now
            };
            var userInfo = new UserInfo
            {
                Gender = "male"
            };

            var respBuilder = new DefaultResponseBuilder();
            var team = respBuilder.BuildTeam(entity, userInfo);

            Assert.AreEqual("pk", team.hackathonName);
            Assert.AreEqual("rk", team.id);
            Assert.AreEqual(false, team.autoApprove.Value);
            Assert.AreEqual("uid", team.creatorId);
            Assert.AreEqual("desc", team.description);
            Assert.AreEqual("dp", team.displayName);
            Assert.AreEqual(entity.CreatedAt, team.createdAt);
            Assert.AreEqual(entity.Timestamp.DateTime, team.updatedAt);
            Assert.AreEqual("male", team.creator.Gender);
        }

        [Test]
        public void BuildTeamMemberTest()
        {
            var entity = new TeamMemberEntity
            {
                PartitionKey = "pk",
                RowKey = "rk",
                HackathonName = "hack",
                Description = "desc",
                Role = TeamMemberRole.Admin,
                Status = TeamMemberStatus.pendingApproval,
                CreatedAt = DateTime.Now,
                Timestamp = DateTime.Now
            };
            var user = new UserInfo
            {
                City = "city",
            };

            var respBuilder = new DefaultResponseBuilder();
            var teamMember = respBuilder.BuildTeamMember(entity, user);

            Assert.AreEqual("hack", teamMember.hackathonName);
            Assert.AreEqual("pk", teamMember.teamId);
            Assert.AreEqual("rk", teamMember.userId);
            Assert.AreEqual("desc", teamMember.description);
            Assert.AreEqual(TeamMemberRole.Admin, teamMember.role);
            Assert.AreEqual(TeamMemberStatus.pendingApproval, teamMember.status);
            Assert.AreEqual(entity.CreatedAt, teamMember.createdAt);
            Assert.AreEqual(entity.Timestamp.DateTime, teamMember.updatedAt);
            Assert.AreEqual("city", teamMember.user.City);
        }

        [Test]
        public void BuildTeamWorkTest()
        {
            var entity = new TeamWorkEntity
            {
                PartitionKey = "pk",
                RowKey = "rk",
                HackathonName = "hack",
                Description = "desc",
                Title = "title",
                Type = TeamWorkType.word,
                Url = "url",
                CreatedAt = DateTime.Now,
                Timestamp = DateTime.Now
            };

            var respBuilder = new DefaultResponseBuilder();
            var teamWork = respBuilder.BuildTeamWork(entity);

            Assert.AreEqual("pk", teamWork.teamId);
            Assert.AreEqual("rk", teamWork.id);
            Assert.AreEqual("hack", teamWork.hackathonName);
            Assert.AreEqual("desc", teamWork.description);
            Assert.AreEqual("title", teamWork.title);
            Assert.AreEqual(TeamWorkType.word, teamWork.type);
            Assert.AreEqual("url", teamWork.url);
            Assert.AreEqual(entity.CreatedAt, teamWork.createdAt);
            Assert.AreEqual(entity.Timestamp.DateTime, teamWork.updatedAt);
        }

        [Test]
        public void BuildAwardAssignmentTest()
        {
            AwardAssignmentEntity awardAssignment = new AwardAssignmentEntity
            {
                PartitionKey = "pk",
                RowKey = "rk",
                AssigneeId = "assignee",
                AwardId = "award",
                Description = "desc",
                CreatedAt = DateTime.UtcNow,
                Timestamp = DateTimeOffset.UtcNow
            };
            var user = new UserInfo { Device = "device" };
            var team = new Team { id = "teamid" };

            var responseBuilder = new DefaultResponseBuilder();
            var result = responseBuilder.BuildAwardAssignment(awardAssignment, team, user);

            Assert.AreEqual("pk", result.hackathonName);
            Assert.AreEqual("rk", result.assignmentId);
            Assert.AreEqual("assignee", result.assigneeId);
            Assert.AreEqual("award", result.awardId);
            Assert.AreEqual("desc", result.description);
            Assert.AreEqual(awardAssignment.CreatedAt, result.createdAt);
            Assert.AreEqual(awardAssignment.Timestamp.DateTime, result.updatedAt);
            Assert.AreEqual("device", result.user.Device);
            Assert.AreEqual("teamid", result.team.id);
        }

        [Test]
        public void BuildResourceListTest()
        {
            string nextLink = "nextlink";
            List<Tuple<EnrollmentEntity, UserInfo>> enrollments = new List<Tuple<EnrollmentEntity, UserInfo>>
            {
                Tuple.Create(new EnrollmentEntity
                {
                    PartitionKey = "pk1",
                    RowKey = "rk1",
                    Status = EnrollmentStatus.approved,
                },
                new UserInfo
                {
                    Id = "uid"
                }),
                Tuple.Create(new EnrollmentEntity
                {
                    PartitionKey = "pk2",
                    RowKey = "rk2",
                    Status = EnrollmentStatus.rejected,
                },
                new UserInfo{
                    Email = "email"
                })
            };

            var builder = new DefaultResponseBuilder();
            var result = builder.BuildResourceList<EnrollmentEntity, UserInfo, Enrollment, EnrollmentList>(
                    enrollments,
                    builder.BuildEnrollment,
                    nextLink);

            Assert.AreEqual("nextlink", result.nextLink);
            Assert.AreEqual(2, result.value.Length);
            Assert.AreEqual("pk1", result.value[0].hackathonName);
            Assert.AreEqual("rk1", result.value[0].userId);
            Assert.AreEqual(EnrollmentStatus.approved, result.value[0].status);
            Assert.AreEqual("uid", result.value[0].user.Id);
            Assert.AreEqual("pk2", result.value[1].hackathonName);
            Assert.AreEqual("rk2", result.value[1].userId);
            Assert.AreEqual(EnrollmentStatus.rejected, result.value[1].status);
            Assert.AreEqual("email", result.value[1].user.Email);
        }
    }
}
