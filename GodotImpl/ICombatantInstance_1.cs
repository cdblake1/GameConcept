using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopDownGame.Data.impl;

namespace TopDownGame
{
		internal interface ICombatantInstance
		{
				public ICombatant Combatant { get; }
		}
}
