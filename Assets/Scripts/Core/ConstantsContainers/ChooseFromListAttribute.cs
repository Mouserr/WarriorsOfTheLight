using System;
using UnityEngine;

namespace Assets.Scripts.Core.ConstantsContainers
{
	public class ChooseFromListAttribute  : PropertyAttribute
	{	
		public ChooseFromListAttribute(Type containerType)
		{
			ContainerType = containerType;
		}

		public Type ContainerType { get; protected set; }
	}
}