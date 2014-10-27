/*
  KeeStats - A plugin for Keepass Password Manager
  Copyright (C) 2014 Andrea Decorte

  This program is free software; you can redistribute it and/or modify
  it under the terms of the GNU General Public License as published by
  the Free Software Foundation; either version 2 of the License, or
  (at your option) any later version.

  This program is distributed in the hope that it will be useful,
  but WITHOUT ANY WARRANTY; without even the implied warranty of
  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
  GNU General Public License for more details.

  You should have received a copy of the GNU General Public License
  along with this program; if not, write to the Free Software
  Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA  02110-1301  USA
 */
using System;
using System.Collections.Generic;
using KeePassLib;
using KeePassLib.Collections;
using NUnit.Framework;

using KeePassLib.Keys;
using KeePassLib.Serialization;
using KeePassLib.Security;

namespace KeeStats.test
{
	[TestFixture]
	public class StatComputerTest
	{
		KeePassLib.PwDatabase _db = null;
		const int normalStatsCount = 9;
		const int qualityStatsCount = 7;
		
		[TestFixtureSetUp]
		public void SetUpTests()
		{
			const string dbpath = "../../test/test.kdbx";
			const string masterpw = "1234";

			var ioConnInfo = new IOConnectionInfo { Path = dbpath };
			var compKey = new CompositeKey();
			compKey.AddUserKey(new KcpPassword(masterpw));

			_db = new KeePassLib.PwDatabase();
			_db.Open(ioConnInfo, compKey, null);
		}
		
		[TestFixtureTearDown]
		public void FixtureTearDown()
		{
			_db.Close();
		}
		
		/// <summary>
		/// One group with one password
		/// </summary>
		[Test]
		public void TestSingleGroup()
		{
			var statsList = new List<StatItem>();
			var qualityStats = new List<ExtendedStatItem>();
			
			PwObjectList<PwGroup> list = _db.RootGroup.GetGroups(true);
			foreach (PwGroup pwgroup in list) {
				if (pwgroup.Name == "Internet") {
					StatComputer.ComputeStats(pwgroup, ref statsList, ref qualityStats, true);
					Assert.AreEqual(normalStatsCount, statsList.Count);
					Assert.AreEqual("Count", statsList[0].Name);
					Assert.AreEqual(1, statsList[3].Value);  // unique pwds
					
					// Shortest and longest is 20
					Assert.AreEqual(20, qualityStats[0].Value);
					Assert.AreEqual(20, qualityStats[1].Value);
				}
			}
		}
		
		/// <summary>
		/// Homebanking is an empty group
		/// </summary>
		[Test]
		public void TestEmptyGroup()
		{
			var statsList = new List<StatItem>();
			var qualityStats = new List<ExtendedStatItem>();
			
			PwObjectList<PwGroup> list = _db.RootGroup.GetGroups(true);
			foreach (PwGroup pwgroup in list) {
				if (pwgroup.Name == "Homebanking") {
					bool result = StatComputer.ComputeStats(pwgroup, ref statsList, ref qualityStats, true);
					Assert.IsFalse(result);
					Assert.AreEqual(0, statsList.Count);
					Assert.AreEqual(0, qualityStats.Count);
				}
			}
		}

		
		/// <summary>
		/// group eMail has 1 entry with empty password -> check the stat
		/// </summary>
		[Test]
		public void TestEmptyPasswords()
		{
			var statsList = new List<StatItem>();
			var qualityStats = new List<ExtendedStatItem>();
			
			PwObjectList<PwGroup> list = _db.RootGroup.GetGroups(true);
			foreach (PwGroup pwgroup in list) {
				if (pwgroup.Name == "eMail") {
					bool result = StatComputer.ComputeStats(pwgroup, ref statsList, ref qualityStats, true);
					Assert.IsTrue(result);
					
					Assert.AreEqual(normalStatsCount, statsList.Count);
					// only an empty password
					Assert.AreEqual(0, qualityStats.Count);
					
					Assert.AreEqual("Unique pwds", statsList[3].Name);
					Assert.AreEqual(0, statsList[3].Value);  // unique pwds
					Assert.AreEqual(1, statsList[2].Value);  // empty pwds
				}
			}
		}

		/// <summary>
		/// Group Network has 3 entries, 2 has the same password
		/// </summary>
		[Test]
		public void TestUniquePasswords()
		{
			var statsList = new List<StatItem>();
			var qualityStats = new List<ExtendedStatItem>();
			
			PwObjectList<PwGroup> list = _db.RootGroup.GetGroups(true);
			foreach (PwGroup pwgroup in list) {
				if (pwgroup.Name == "Network") {
					bool result = StatComputer.ComputeStats(pwgroup, ref statsList, ref qualityStats, true);
					Assert.IsTrue(result);
					
					Assert.AreEqual(normalStatsCount, statsList.Count);
					Assert.AreEqual(qualityStatsCount, qualityStats.Count);
					
					Assert.AreEqual("Count", statsList[0].Name);
					Assert.AreEqual(3, statsList[0].Value);
					
					Assert.AreEqual("Unique pwds", statsList[3].Name);
					Assert.AreEqual(2, statsList[3].Value);  // unique pwds
					Assert.AreEqual(0, statsList[2].Value);  // empty pwds
					
					Assert.AreEqual("Average length", statsList[5].Name);
					Assert.AreEqual(3.5, statsList[5].Value);
					
					Assert.AreEqual("Percentage of entries with an URL", statsList[6].Name);
					Assert.AreEqual(0, statsList[6].Value);
					
					Assert.AreEqual("Referenced passwords", statsList[7].Name);
					Assert.AreEqual(0, statsList[7].Value);
					
					Assert.AreEqual("Shortest password", qualityStats[0].Name);
					Assert.AreEqual(3, qualityStats[0].Value);
					
					Assert.AreEqual("Longest password", qualityStats[1].Name);
					Assert.AreEqual(4, qualityStats[1].Value);
					
					
				}
			}
		}
		
		
		/// <summary>
		/// Group Windows has 1 entry REFTEST whose password refers to Network/test1 password
		/// </summary>
		[Test]
		public void TestReferencedPasswords()
		{
			var statsList = new List<StatItem>();
			var qualityStats = new List<ExtendedStatItem>();
			
			PwObjectList<PwGroup> list = _db.RootGroup.GetGroups(true);
			foreach (PwGroup pwgroup in list) {
				if (pwgroup.Name == "Windows") {
					bool result = StatComputer.ComputeStats(pwgroup, ref statsList, ref qualityStats, true);
					
					Assert.IsTrue(result);
					
					Assert.AreEqual(normalStatsCount, statsList.Count);
					Assert.AreEqual(0, qualityStats.Count);
					
					Assert.AreEqual("Count", statsList[0].Name);
					Assert.AreEqual(1, statsList[0].Value);
					
					Assert.AreEqual("Referenced passwords", statsList[7].Name);
					Assert.AreEqual(1, statsList[7].Value);
				}
			}
		}
		
		[Test]
		public void TestQualityOfPasswords()
		{
			// Create group and pass to the calculate method
			PwGroup subgroup = new PwGroup(true, true, "Quality test", new PwIcon());
			PwEntry entry1 = new PwEntry(true, true);
			entry1.Strings.Set(PwDefs.TitleField, new ProtectedString(false, "Numeric"));
			entry1.Strings.Set(PwDefs.PasswordField, new ProtectedString(false, "111"));
			subgroup.AddEntry(entry1, true);
			
			PwEntry entry2 = new PwEntry(true, true);
			entry2.Strings.Set(PwDefs.TitleField, new ProtectedString(false, "AlphaNumeric"));
			entry2.Strings.Set(PwDefs.PasswordField, new ProtectedString(false, "111ae"));
			subgroup.AddEntry(entry2, true);
			
			PwEntry entry3 = new PwEntry(true, true);
			entry3.Strings.Set(PwDefs.TitleField, new ProtectedString(false, "Special chars"));
			entry3.Strings.Set(PwDefs.PasswordField, new ProtectedString(false, "$&**"));
			subgroup.AddEntry(entry3, true);
			
			PwEntry entry4 = new PwEntry(true, true);
			entry4.Strings.Set(PwDefs.TitleField, new ProtectedString(false, "Upper"));
			entry4.Strings.Set(PwDefs.PasswordField, new ProtectedString(false, "AAAA"));
			subgroup.AddEntry(entry4, true);
			
			
			PwEntry entry5 = new PwEntry(true, true);
			entry5.Strings.Set(PwDefs.TitleField, new ProtectedString(false, "Lower"));
			entry5.Strings.Set(PwDefs.PasswordField, new ProtectedString(false, "bbbb"));
			subgroup.AddEntry(entry5, true);
			
			PwEntry entry6 = new PwEntry(true, true);
			entry6.Strings.Set(PwDefs.TitleField, new ProtectedString(false, "Upperlower"));
			entry6.Strings.Set(PwDefs.PasswordField, new ProtectedString(false, "bbbbAAA"));
			subgroup.AddEntry(entry6, true);
			//_db.RootGroup.AddGroup(subgroup, true);
			
			var statsList = new List<StatItem>();
			var qualityStats = new List<ExtendedStatItem>();
			
			bool result = StatComputer.ComputeStats(subgroup, ref statsList, ref qualityStats, true);
			
			Assert.IsTrue(result);
			
			Assert.AreEqual(normalStatsCount, statsList.Count);
			Assert.AreEqual(qualityStatsCount, qualityStats.Count);
			
			Assert.AreEqual("Count", statsList[0].Name);
			Assert.AreEqual(6, statsList[0].Value);
			
			
			Assert.AreEqual("Unique pwds", statsList[3].Name);
			Assert.AreEqual(6, statsList[3].Value);  // unique pwds
			Assert.AreEqual(0, statsList[2].Value);  // empty pwds
			
			Assert.AreEqual("Average length", statsList[5].Name);
			Assert.AreEqual(4.5, statsList[5].Value);
			
			Assert.AreEqual("Percentage of entries with an URL", statsList[6].Name);
			Assert.AreEqual(0, statsList[6].Value);
			
			Assert.AreEqual("Referenced passwords", statsList[7].Name);
			Assert.AreEqual(0, statsList[7].Value);
			
			Assert.AreEqual("Shortest password", qualityStats[0].Name);
			Assert.AreEqual(3, qualityStats[0].Value);
			
			Assert.AreEqual("Longest password", qualityStats[1].Name);
			Assert.AreEqual(7, qualityStats[1].Value);
			
			Assert.AreEqual("Lowercase only passwords", qualityStats[2].Name);
			Assert.AreEqual(1, qualityStats[2].Value);
			
			Assert.AreEqual("Uppercase only passwords", qualityStats[3].Name);
			Assert.AreEqual(1, qualityStats[3].Value);
			
			Assert.AreEqual("Numeric only passwords", qualityStats[4].Name);
			Assert.AreEqual(1, qualityStats[4].Value);
			
			Assert.AreEqual("Alphanumeric only passwords", qualityStats[5].Name);
			Assert.AreEqual(1, qualityStats[5].Value);
			
			Assert.AreEqual("Not alphanumeric passwords", qualityStats[6].Name);
			Assert.AreEqual(1, qualityStats[6].Value);
		}
		
		[Test]
		public void TestRecentPasswords()
		{
			// Create group and pass to the calculate method
			PwGroup subgroup = new PwGroup(true, true, "Time test", new PwIcon());
			PwEntry entry1 = new PwEntry(true, true);
			entry1.LastAccessTime = DateTime.Now.Subtract(TimeSpan.FromDays(40));
			entry1.Strings.Set(PwDefs.TitleField, new ProtectedString(false, "Numeric"));
			entry1.Strings.Set(PwDefs.PasswordField, new ProtectedString(false, "111"));
			subgroup.AddEntry(entry1, true);
			
			PwEntry entry2 = new PwEntry(true, true);
			entry2.LastAccessTime = DateTime.Now.Subtract(TimeSpan.FromDays(20));
			entry2.Strings.Set(PwDefs.TitleField, new ProtectedString(false, "AlphaNumeric"));
			entry2.Strings.Set(PwDefs.PasswordField, new ProtectedString(false, "111ae"));
			subgroup.AddEntry(entry2, true);
			
			PwEntry entry3 = new PwEntry(true, true);
			entry3.LastAccessTime = DateTime.Now;
			entry3.Strings.Set(PwDefs.TitleField, new ProtectedString(false, "Special chars"));
			entry3.Strings.Set(PwDefs.PasswordField, new ProtectedString(false, "$&**"));
			subgroup.AddEntry(entry3, true);
			
			PwEntry entry4 = new PwEntry(true, true);
			entry4.LastAccessTime = DateTime.Now.Subtract(TimeSpan.FromDays(300));
			entry4.Strings.Set(PwDefs.TitleField, new ProtectedString(false, "Upper"));
			entry4.Strings.Set(PwDefs.PasswordField, new ProtectedString(false, "AAAA"));
			subgroup.AddEntry(entry4, true);
			
			var statsList = new List<StatItem>();
			var qualityStats = new List<ExtendedStatItem>();
			
			bool result = StatComputer.ComputeStats(subgroup, ref statsList, ref qualityStats, true);
			Assert.IsTrue(result);
			
			Assert.AreEqual(normalStatsCount, statsList.Count);
			Assert.AreEqual(qualityStatsCount, qualityStats.Count);
			
			Assert.AreEqual(normalStatsCount, statsList.Count);
			Assert.AreEqual(qualityStatsCount, qualityStats.Count);
			
			Assert.AreEqual("Count", statsList[0].Name);
			Assert.AreEqual(4, statsList[0].Value);
			
			Assert.AreEqual("% of passwords accessed recently", statsList[8].Name);
			Assert.AreEqual(50, statsList[8].Value);			
		}
		
	}
}
