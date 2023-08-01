﻿using System;
using System.Collections.Generic;
using System.Text;
using Blah.Features;
using Blah.Ordering;
using Blah.Reflection;
using UnityEditor;
using UnityEngine;

namespace Blah.Editor
{
internal static class BlahEditorSystemsOrdering
{
	[MenuItem("Blah/Report/Systems ordering issues")]
	public static void ReportIssues()
	{
		var sb = new StringBuilder();
		sb.AppendLine("--- systems ordering issues ---");
		
		var context = BlahEditorHelper.FindGameContext();

		var featuresBySystemsGroups =
			(Dictionary<int, List<BlahFeatureBase>>)BlahReflection.GetContextFeaturesBySystemsGroups(context);
		foreach ((int groupId, var features) in featuresBySystemsGroups)
		{
			var systems = new List<Type>();
			foreach (var feature in features)
				if (feature.Systems != null)
					foreach (var system in feature.Systems)
						systems.Add(system);

			try
			{
				BlahOrderer.Order(ref systems);
			}
			catch (BlahOrdererSortingException e)
			{
				sb.AppendLine($"group {groupId}, {e.GetFullMsg()}");
			}
		}

		sb.AppendLine("-----------------------");
		Debug.Log(sb);
	}
}
}