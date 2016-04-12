using System;

namespace Guillotine.Menu
{
	public interface IGuillotineListener
	{
		void OnGuillotineOpened();
		void OnGuillotineClosed();
	}
}

