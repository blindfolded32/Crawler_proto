using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Engine;

namespace Selection
{
	public class SelectObjectsController : IGUIUpdatable, IController
	{

		private List<ISelectableUnit> _allSelectableUnits; // массив всех юнитов, которых мы можем выделить
		private List<ISelectableUnit> _unitsSelected; // массив выделенных юнитов

		public List<ISelectableUnit> UnitsSelected => _unitsSelected;

		public GUISkin _skin;
		private Rect _rect;
		private bool _draw;
		private Vector2 _startPos;
		private Vector2 _endPos;

		public SelectObjectsController(List<ISelectableUnit> allSelectableUnits)
		{
			_allSelectableUnits = allSelectableUnits;
			_unitsSelected = new List<ISelectableUnit>();
		}

		// проверка, добавлен объект или нет
		bool CheckUnit(ISelectableUnit unit)
		{
			bool result = false;
			foreach (var u in _unitsSelected)
			{
				if (u == unit) result = true;
			}
			return result;
		}

		void Select()
		{
			if (_unitsSelected.Count > 0)
			{
				for (int j = 0; j < _unitsSelected.Count; j++)
				{
					// делаем что-либо с выделенными объектами
					_unitsSelected[j].MeshRenderer.material.color = Color.red;
				}
			}
		}

		void Deselect()
		{
			if (_unitsSelected.Count > 0)
			{
				for (int j = 0; j < _unitsSelected.Count; j++)
				{
					// отменяем то, что делали с объектоми
					_unitsSelected[j].MeshRenderer.material.color = Color.white;
				}
			}
		}

		public void localOnGUI()
		{
			GUI.skin = _skin;
			GUI.depth = 99;

			if (Input.GetMouseButtonDown(0))
			{
				Deselect();
				_startPos = Input.mousePosition;
				_draw = true;
			}

			if (Input.GetMouseButtonUp(0))
			{
				_draw = false;
				Select();
			}

			if (_draw)
			{
				_unitsSelected.Clear();
				_endPos = Input.mousePosition;
				if (_startPos == _endPos) return;

				_rect = new Rect(Mathf.Min(_endPos.x, _startPos.x),
								Screen.height - Mathf.Max(_endPos.y, _startPos.y),
								Mathf.Max(_endPos.x, _startPos.x) - Mathf.Min(_endPos.x, _startPos.x),
								Mathf.Max(_endPos.y, _startPos.y) - Mathf.Min(_endPos.y, _startPos.y)
								);

				GUI.Box(_rect, "");

				for (int j = 0; j < _allSelectableUnits.Count; j++)
				{
					// трансформируем позицию объекта из мирового пространства, в пространство экрана
					Vector2 tmp = new Vector2(Camera.main.WorldToScreenPoint(_allSelectableUnits[j].UnitTransform.position).x, Screen.height - Camera.main.WorldToScreenPoint(_allSelectableUnits[j].UnitTransform.position).y);

					if (_rect.Contains(tmp)) // проверка, находится-ли текущий объект в рамке
					{
						if (_unitsSelected.Count == 0)
						{
							_unitsSelected.Add(_allSelectableUnits[j]);
						}
						else if (!CheckUnit(_allSelectableUnits[j]))
						{
							_unitsSelected.Add(_allSelectableUnits[j]);
						}
					}
				}
			}
		}
    }
}