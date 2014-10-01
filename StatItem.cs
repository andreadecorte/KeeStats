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

using KeePass.Plugins;
using KeePass.Forms;
using KeePassLib;

namespace KeeStats
{
	/// <summary>
	/// A simple class to store a statistic item.
	/// </summary>
	public class StatItem
	{
		public StatItem(string name, float value)
		{
			_name = name;
			_value = value;
		}

		public string _name;
		public string Name { get { return _name; } set { _name = value; } }
		public float _value;
		public float Value { get { return _value; } set { _value = value; } }
		
	}
	
	/// <summary>
	/// This class stores also a reference to a single password entry
	/// </summary>
	public class ExtendedStatItem : StatItem
	{
		public ExtendedStatItem(string name, float value, PwEntry item) : base(name, value)
		{
			_item = item;
		}
		
		public PwEntry _item;
		public PwEntry Item { get { return _item; } set { _item = value; } }		
	}
}
