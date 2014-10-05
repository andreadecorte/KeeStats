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

namespace KeeStats
{
	/// <summary>
	/// A static class which takes care of statistic computation process
	/// </summary>
	public static class StatComputer
	{
		/// <summary>
		/// Computes the statistics
		/// </summary>
		/// <param name="group">The group on which to calculate the statistics</param>
		/// <param name="genericStats">List that will be populated with generic stats</param>
		/// <param name="qualityStats">Extended quality stats</param>
		/// <param name="isRecursive">Whether the stats should also be calculated on subfolders</param>
		/// <returns>False if there are no password in the selected group</returns>
		public static bool ComputeStats(PwGroup group, ref List<StatItem> genericStats, ref List<ExtendedStatItem> qualityStats, bool isRecursive)
		{
			// average length for unique passwords + distribution
			// most common?
			// including only numbers, alpha, uppercase, special chars
			// Average of last access ?
			uint totalNumber = group.GetEntriesCount(isRecursive);
			
			if (totalNumber == 0) {
				return false;
			}
			
			genericStats.Add(new StatItem("Count", group.GetEntriesCount(isRecursive)));
			genericStats.Add(new StatItem("Number of groups", group.GetGroups(isRecursive).UCount));
			
			// HashSet
			int shortestLength = 1000;
			string shortestPass = "";
			PwEntry shortestEntry = null;
			int longestLength = 0;
			string longestPass = "";
			PwEntry longestEntry = null;
			
			int emptyPasswords = 0;
			int totalLength = 0;
			
			Dictionary<string, PwEntry> passwords = new Dictionary<string, PwEntry>();
			foreach (PwEntry aPasswordObject in group.GetEntries(isRecursive)) {
				try {
					string thePasswordString = aPasswordObject.Strings.ReadSafe(PwDefs.PasswordField);
					int thePasswordStringLength = thePasswordString.Length;
					// special case for empty passwords
					if (thePasswordStringLength == 0) {
						emptyPasswords++;
						continue;
					}
					
					passwords.Add(thePasswordString, aPasswordObject);
					if (thePasswordStringLength < shortestLength) {
						shortestPass = thePasswordString;
						shortestLength = thePasswordStringLength;
						shortestEntry = aPasswordObject;
					}
					if (thePasswordStringLength > longestLength) {
						longestPass = thePasswordString;
						longestLength = thePasswordStringLength;
						longestEntry = aPasswordObject;
					}
					
					totalLength += thePasswordStringLength;
					
				} catch(ArgumentException) {
					// We want only unique passwords, so don't do anything
					// Code coverage ignores this for some reason...
				}
			}
			
			genericStats.Add(new StatItem("Empty passwords", emptyPasswords));
			genericStats.Add(new StatItem("Unique pwds", passwords.Count));
			genericStats.Add(new StatItem("% of unique pwds", (passwords.Count/(float) totalNumber)*100));
			genericStats.Add(new StatItem("Average length", (totalLength / (float) passwords.Count)));
			
			// if 0, we didn't see have any password
			if (longestLength > 0) {
				qualityStats.Add(new ExtendedStatItem("Shortest password", shortestLength, shortestEntry));
				qualityStats.Add(new ExtendedStatItem("Longest password", longestLength, longestEntry));
			}
			
			return true;
		}
	}
	
}
