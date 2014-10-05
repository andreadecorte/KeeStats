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

namespace KeeStats.test
{
	[TestFixture]
	public class StatComputerTest
	{
		KeePassLib.PwDatabase _db = null;
		
		[TestFixtureSetUp]
		public void SetUpTests()
		{
			var dbpath = "../../test/test.kdbx";
			var masterpw = "1234";

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
					Assert.AreEqual(6, statsList.Count);
					Assert.AreEqual("Count", statsList[0].Name);
					Assert.AreEqual(1, statsList[3].Value);  // unique pwds
					
					// Shortest and longest is 20
					Assert.AreEqual(20, qualityStats[0].Value);
					Assert.AreEqual(20, qualityStats[1].Value);
				}
			}
		}
		
		/// <summary>
		/// An empty group
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
					
					Assert.AreEqual(6, statsList.Count);
					// only an empty password
					Assert.AreEqual(0, qualityStats.Count);
					
					Assert.AreEqual("Unique pwds", statsList[3].Name);
					Assert.AreEqual(0, statsList[3].Value);  // unique pwds
					Assert.AreEqual(1, statsList[2].Value);  // empty pwds
				}
			}
		}

		/// <summary>
		/// Group networking has 3 entries, 2 has the same password
		/// </summary>
		[Test]
		public void TestUniquePasswords()
		{
			var statsList = new List<StatItem>();
			var qualityStats = new List<ExtendedStatItem>();
			
			PwObjectList<PwGroup> list = _db.RootGroup.GetGroups(true);
			foreach (PwGroup pwgroup in list) {
				if (pwgroup.Name == "Networking") {
					bool result = StatComputer.ComputeStats(pwgroup, ref statsList, ref qualityStats, true);
					Assert.IsTrue(result);
					
					Assert.AreEqual(5, statsList.Count);
					Assert.AreEqual(2, qualityStats.Count);
					
					Assert.AreEqual("Count", statsList[0].Name);
					Assert.AreEqual(3, statsList[0].Value);
					
					Assert.AreEqual("Unique pwds", statsList[3].Name);
					Assert.AreEqual(2, statsList[3].Value);  // unique pwds
					Assert.AreEqual(0, statsList[2].Value);  // empty pwds
					
					Assert.AreEqual("Average length", statsList[5].Name);  // empty pwds
					Assert.AreEqual(3.5, statsList[5].Value);  // empty pwds
					
					Assert.AreEqual("Shortest password", qualityStats[0].Name);
					Assert.AreEqual(3, qualityStats[0].Value);
					
					Assert.AreEqual("Longest password", qualityStats[1].Name);
					Assert.AreEqual(4, qualityStats[1].Value);
				}
			}
		}
		// TODO test windows? (ex. changed checkbox)
	}
}
