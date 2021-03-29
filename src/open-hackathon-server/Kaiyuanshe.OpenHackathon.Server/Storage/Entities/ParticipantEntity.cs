﻿using Kaiyuanshe.OpenHackathon.Server.Models;
using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace Kaiyuanshe.OpenHackathon.Server.Storage.Entities
{
    /// <summary>
    /// Represents a hackathon participant. Could be admin, organizer, partner, judge and so on.
    /// 
    /// PK: hackathon Name. 
    /// RK: user Id.
    /// 
    /// PK might be string.Empty for PlatformAdministrator.
    /// </summary>
    public class ParticipantEntity : AdvancedTableEntity
    {
        [IgnoreProperty]
        public string HackathonName
        {
            get
            {
                return PartitionKey;
            }
        }

        [IgnoreProperty]
        public string UserId
        {
            get
            {
                return RowKey;
            }
        }

        public ParticipantRole Role { get; set; }

        public EnrollmentStatus Status { get; set; }

        public DateTime CreatedAt { get; set; }

        public bool IsPlatformAdministrator()
        {
            return string.IsNullOrEmpty(HackathonName) && Role.HasFlag(ParticipantRole.Administrator);
        }

        public bool IsContestant()
        {
            return Role.HasFlag(ParticipantRole.Contestant);
        }
    }

    [Flags]
    public enum ParticipantRole
    {
        None = 0,
        Administrator = 1,
        Judge = 2,
        Contestant = 4,
    }
}
