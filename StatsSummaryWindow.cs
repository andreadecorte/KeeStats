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
using System.Windows.Forms;

using System.Collections.Generic;

using KeePassLib;
using KeePass.Forms;

namespace KeeStats
{
	/// <summary>
	/// Show the computed statistics in two datagridviews
	/// </summary>
	public partial class StatsSummaryWindow : Form
	{
		private PwDatabase _database = null;
		public PwDatabase Database { get { return _database; } set { _database = value; } }
		
		private PwGroup _group = null;
		public PwGroup Group { get { return _group; } set
			{ _group = value;
				// add group name in window title
				this.Text += " for group: ";
				this.Text += _group.Name;
			} }
		
		private ImageList _icons = null;
		public ImageList Icons { get { return _icons; } set { _icons = value; } }

		
		public StatsSummaryWindow(List<StatItem> items, List<ExtendedStatItem> extended_items)
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();

			generalStatsView.DataSource = items;
			generalStatsView.ReadOnly = true;
			
			qualityStatsView.DataSource = extended_items;
			qualityStatsView.ReadOnly = true;
			
			// Disable if no items
			generalStatsView.Enabled = items.Count != 0;
			qualityStatsView.Enabled = extended_items.Count != 0;
			
			// Hide item object column
			qualityStatsView.Columns["Item"].Visible = false;
			// Try to hide it on Mono (Visible properties not respected)
			qualityStatsView.Columns["Item"].Width = 0;
			qualityStatsView.Refresh();
			
			qualityStatsView.CellClick += new DataGridViewCellEventHandler(qualityStatsView_CellClick);
			
			recursiveSearch.Checked = true;
			recursiveSearch.CheckedChanged += HandleCheckedChanged;
		}
		
		private void qualityStatsView_CellClick(object sender, DataGridViewCellEventArgs e)
		{
			if (e.ColumnIndex !=  qualityStatsView.Rows[e.RowIndex].Cells["Value"].ColumnIndex)
			{
				// only show if we're clicking on Value cell
				return;
			}
			
			if (_database == null) {
				// we need the database to show the edit window
				return;
			}
			
			if (_icons == null) {
				return;
			}

			PwEntry entry = qualityStatsView.Rows[e.RowIndex].Cells["Item"].Value as PwEntry;
			
			if (entry == null) {
				// For example a stat without an entry associated
				return;
			}
			
			try {
				PwEntryForm aForm = new PwEntryForm();
				aForm.InitEx(entry, PwEditMode.ViewReadOnlyEntry, _database, _icons, false, false);
				aForm.Show();
			} catch (Exception ex) {
				MessageBox.Show("Error while loading the edit window: " + ex.Message, "KeeStats", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}

			// TODO fix the Cancel button not working despite adding the event
		}

		void HandleCheckedChanged(object sender, EventArgs e)
		{
			CheckBox cb = sender as CheckBox;
			List<StatItem> items = new List<StatItem>();
			List<ExtendedStatItem> extended_items = new List<ExtendedStatItem>();
			
			// Recompute stats
			var result = StatComputer.ComputeStats(_group, ref items, ref extended_items, cb.Checked);
			
			// Now update the data source so the view is updated
			generalStatsView.DataSource = items;
			qualityStatsView.DataSource = extended_items;
			
			// Disable if no items
			generalStatsView.Enabled = items.Count != 0;
			qualityStatsView.Enabled = extended_items.Count != 0;
		}
	}
}
