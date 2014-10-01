/*
 * Created by SharpDevelop.
 * User: Andri
 * Date: 09/09/2014
 * Time: 23:09
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace KeeStats
{
	partial class StatsSummaryWindow
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.TabControl tabControl;
		private System.Windows.Forms.TabPage tabGeneral;
		private System.Windows.Forms.TabPage tabQuality;
		private System.Windows.Forms.DataGridView generalStatsView;
		private System.Windows.Forms.DataGridView qualityStatsView;
		
		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
			this.tabControl = new System.Windows.Forms.TabControl();
			this.tabGeneral = new System.Windows.Forms.TabPage();
			this.generalStatsView = new System.Windows.Forms.DataGridView();
			this.tabQuality = new System.Windows.Forms.TabPage();
			this.qualityStatsView = new System.Windows.Forms.DataGridView();
			this.tabControl.SuspendLayout();
			this.tabGeneral.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.generalStatsView)).BeginInit();
			this.tabQuality.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.qualityStatsView)).BeginInit();
			this.SuspendLayout();
			// 
			// tabControl
			// 
			this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
			| System.Windows.Forms.AnchorStyles.Left) 
			| System.Windows.Forms.AnchorStyles.Right)));
			this.tabControl.Controls.Add(this.tabGeneral);
			this.tabControl.Controls.Add(this.tabQuality);
			this.tabControl.Location = new System.Drawing.Point(0, 1);
			this.tabControl.Name = "tabControl";
			this.tabControl.SelectedIndex = 0;
			this.tabControl.Size = new System.Drawing.Size(284, 260);
			this.tabControl.TabIndex = 0;
			// 
			// tabGeneral
			// 
			this.tabGeneral.Controls.Add(this.generalStatsView);
			this.tabGeneral.Location = new System.Drawing.Point(4, 22);
			this.tabGeneral.Name = "tabGeneral";
			this.tabGeneral.Padding = new System.Windows.Forms.Padding(3);
			this.tabGeneral.Size = new System.Drawing.Size(276, 234);
			this.tabGeneral.TabIndex = 0;
			this.tabGeneral.Text = "General Stats";
			this.tabGeneral.UseVisualStyleBackColor = true;
			// 
			// generalStatsView
			// 
			this.generalStatsView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
			| System.Windows.Forms.AnchorStyles.Left) 
			| System.Windows.Forms.AnchorStyles.Right)));
			this.generalStatsView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.generalStatsView.GridColor = System.Drawing.SystemColors.ActiveCaption;
			this.generalStatsView.Location = new System.Drawing.Point(3, 3);
			this.generalStatsView.Name = "generalStatsView";
			this.generalStatsView.RowHeadersVisible = false;
			this.generalStatsView.ShowEditingIcon = false;
			this.generalStatsView.Size = new System.Drawing.Size(270, 228);
			this.generalStatsView.TabIndex = 0;
			// 
			// tabQuality
			// 
			this.tabQuality.Controls.Add(this.qualityStatsView);
			this.tabQuality.Location = new System.Drawing.Point(4, 22);
			this.tabQuality.Name = "tabQuality";
			this.tabQuality.Padding = new System.Windows.Forms.Padding(3);
			this.tabQuality.Size = new System.Drawing.Size(276, 234);
			this.tabQuality.TabIndex = 1;
			this.tabQuality.Text = "Quality Stats";
			this.tabQuality.UseVisualStyleBackColor = true;
			// 
			// qualityStatsView
			// 
			this.qualityStatsView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.qualityStatsView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.qualityStatsView.GridColor = System.Drawing.SystemColors.ActiveCaption;
			this.qualityStatsView.Location = new System.Drawing.Point(3, 3);
			this.qualityStatsView.Name = "qualityStatsView";
			this.qualityStatsView.RowHeadersVisible = false;
			this.qualityStatsView.ShowEditingIcon = false;
			this.qualityStatsView.Size = new System.Drawing.Size(270, 228);
			this.qualityStatsView.TabIndex = 0;
			// 
			// StatsSummaryWindow
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(284, 262);
			this.Controls.Add(this.tabControl);
			this.Name = "StatsSummaryWindow";
			this.Text = "KeeStats";
			this.tabControl.ResumeLayout(false);
			this.tabGeneral.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.generalStatsView)).EndInit();
			this.tabQuality.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.qualityStatsView)).EndInit();
			this.ResumeLayout(false);

		}
	}
}
