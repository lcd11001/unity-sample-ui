using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public partial class MenuController : MonoBehaviour 
{
	public MenuItem menu;
	private MenuItem firstCircleItem;
	LinkedList<Anim> animSequence;
	List<Anim> animParallel;
	List<LinkedList<Anim>> animParallelSequence;

	void Awake() 
	{
		animSequence = new LinkedList<Anim>();
		animParallel = new List<Anim>();
		animParallelSequence = new List< LinkedList<Anim> >();

		firstCircleItem = null;

		float deltaAngle = 360.0f / menu.children.Count;
		int i = 0;
		foreach(MenuItem child in menu.children)
        {
			child.angle = i * deltaAngle;
            i ++;
        }
	}
	
	// Update is called once per frame
	void Update () 
	{
		UpdateSequence(animSequence);
		UpdateParallel(animParallel);
		UpdateParallelSequence(animParallelSequence);
	}

	void UpdateSequence(LinkedList<Anim> sequence)
	{
		if (sequence.First != null)
		{
			Anim anim = sequence.First.Value;
			
			if (anim.Ratio < 1)
			{
				anim.Update();
			}
			else
			{
				sequence.RemoveFirst();
			}
		}
	}

	void UpdateParallel(List<Anim> parallel)
	{
		int size = parallel.Count;
		if (size > 0)
		{
			int i = 0;
			while(i < size)
			{
				Anim anim = parallel[i];
				if (anim.Ratio < 1)
				{
					anim.Update();
					i ++;
				}
				else
				{
					parallel.RemoveAt(i);
					size -= 1;
				}
			}
		}
	}

	void UpdateParallelSequence(List< LinkedList<Anim> > parallel_sequence)
	{
		int size = parallel_sequence.Count;
		if (size > 0)
		{
			int i = 0;
			while(i < size)
			{
				LinkedList<Anim> sequence = parallel_sequence[i];
				UpdateSequence(sequence);

				if (sequence.Count == 0)
				{
					parallel_sequence.RemoveAt(i);
					size --;
				}
				else
				{
					i ++;
				}
			}
		}
	}

	public void StartFadeIn(Button button)
	{
		MenuItem item  = FindItemFrom(button);
		if (item == null)
		{
			Debug.LogError("StartFadeIn fail " + button.name);
			return;
		}
		AnimFadeIn a = new AnimFadeIn();
		if (a.StartAnim(item))
		{
			lock(animParallel)
			{
				animParallel.Add(a);
			}
		}
	}

	public void StartFadeOut(Button button)
	{
		MenuItem item  = FindItemFrom(button);
		if (item == null)
		{
			Debug.LogError("StartFadeOut fail " + button.name);
			return;
		}
		AnimFadeOut a = new AnimFadeOut();
		if (a.StartAnim(item))
		{
			lock(animParallel)
			{
				animParallel.Add(a);
			}
		}
	}

	public void StartExpand(Button button)
	{
		MenuItem item  = FindItemFrom(button);
		if (item == null)
		{
			Debug.LogError("StartExpand fail " + button.name);
			return;
		}
		AnimExpand a = new AnimExpand();
		if (a.StartAnim(item))
		{
			lock(animParallel)
			{
				animParallel.Add(a);
			}
		}
	}

	public void StartCollapse(Button button)
	{
		MenuItem item  = FindItemFrom(button);
		if (item == null)
		{
			Debug.LogError("StartCollapse fail " + button.name);
			return;
		}


		bool hasSequence = false;
		if (firstCircleItem != null)
		{
			AnimSlideIn b = new AnimSlideIn();
			if (b.StartAnim(firstCircleItem))
			{
				hasSequence = true;
				LinkedList<Anim> sequence = new LinkedList<Anim>();

				lock(animParallelSequence)
				{
					sequence.AddLast(b);

					AnimCollapse c = new AnimCollapse();
					if (c.StartAnim(item))
					{
						sequence.AddLast(c);
					}

					animParallelSequence.Add(sequence);
				}
				
			}

			firstCircleItem = null;
		}
		
		
		if (!hasSequence)
		{
			AnimCollapse a = new AnimCollapse();
			if (a.StartAnim(item))
			{
				lock(animParallel)
				{
					animParallel.Add(a);
				}
			}
		}
	}

	public void StartSlideIn(Button button)
	{
		ForceStartSlideIn(button, false);
	}

	private void ForceStartSlideIn(Button button, bool force)
	{
		MenuItem item  = FindItemFrom(button);
		if (item == null)
		{
			Debug.LogError("StartSlideIn fail " + button.name);
			return;
		}

		if (!force && firstCircleItem != item)
		{
			Debug.LogWarning("StartSlideIn not selected " + button.name);
			return;
		}

		AnimSlideIn a = new AnimSlideIn();
		if (a.StartAnim(item))
		{
			lock(animParallel)
			{
				animParallel.Add(a);
			}
		}
	}

	public void StartSlideOut(Button button)
	{
		ForceStartSlideOut(button, false);
	}

	private void ForceStartSlideOut(Button button, bool force)
	{
		MenuItem item  = FindItemFrom(button);
		if (item == null)
		{
			Debug.LogError("StartSlideOut fail " + button.name);
			return;
		}

		if (!force && firstCircleItem != item)
		{
			Debug.LogWarning("StartSlideOut not selected " + button.name);
			return;
		}

		AnimSlideOut a = new AnimSlideOut();
		if (a.StartAnim(item))
		{
			lock(animParallel)
			{
				animParallel.Add(a);
			}
		}
	}

	public void StartRotate(Button button)
	{
		// update circle
		MenuItem selected = FindItemFrom(button);
		if (firstCircleItem == selected)
		{
			Debug.LogWarning("StartRotate cancel " + button.name);
			return;
		}
		// if (firstCircleItem != null)
		// {
		// 	ForceStartSlideIn(firstCircleItem.button, true);
		// }
		
		firstCircleItem = selected;

		MenuItem parent = FindParentFrom(button);
		float offsetAngle = parent != null ? parent.angle : 0;
		float localAngle = Vector3.SignedAngle(button.transform.localPosition, Vector3.right, Vector3.back);
		float deltaAngle = 90 - localAngle + offsetAngle;

		List<MenuItem> list = FindSiblingFrom(button);
		
		foreach(MenuItem child in list)
		{
			localAngle = Vector3.SignedAngle(child.button.transform.localPosition, Vector3.right, Vector3.back);

			child.startAngle = localAngle;
			child.endAngle = localAngle + deltaAngle;
			child.angle = child.endAngle - offsetAngle;

			if (child == firstCircleItem)
			{
				LinkedList<Anim> sequence = new LinkedList<Anim>();
				AnimRotate rotate = new AnimRotate();
				if (rotate.StartAnim(child))
				{
					sequence.AddLast(rotate);
				}

				AnimSlideOut slideOut = new AnimSlideOut();
				if (slideOut.StartAnim(child))
				{
					sequence.AddLast(slideOut);
				}

				lock(animParallelSequence)
				{
					animParallelSequence.Add(sequence);
				}
			}
			else
			{
				AnimRotate a = new AnimRotate();
				if (a.StartAnim(child))
				{
					lock(animParallel)
					{
						animParallel.Add(a);
					}
				}

				ForceStartSlideIn(child.button, true);
			}
		}
		
	}

	MenuItem FindItemFrom(Button button)
	{
		return FindItemFrom(menu, button);
	}

	MenuItem FindItemFrom(MenuItem root, Button button)
	{
		// Debug.Log("root " + root.button.name + " vs button " + button.name);
		if (button == root.button)
		{
			return root;
		}

		foreach(MenuItem child in root.children)
		{
			MenuItem found = FindItemFrom(child, button);
			if (found != null)
			{
				return found;
			}
		}

		return null;
	}

	List<MenuItem> FindSiblingFrom(Button button)
	{
		return FindSiblingFrom(menu, button);
	}

	List<MenuItem> FindSiblingFrom(MenuItem parent, Button button)
	{
		if (button == parent.button)
		{
			return null;
		}

		foreach(MenuItem child in parent.children)
		{
			if (child.button == button)
			{
				return parent.children;
			}

			List<MenuItem> found = FindSiblingFrom(child, button);
			if (found != null)
			{
				return found;
			}
		}

		return null;
	}

	MenuItem FindParentFrom(Button button)
	{
		return FindParentFrom(menu, button);
	}

	MenuItem FindParentFrom(MenuItem root, Button button)
	{
		// Debug.Log("root " + root.button.name + " vs button " + button.name);
		if (button == root.button)
		{
			return null;
		}

		foreach(MenuItem child in root.children)
		{
			if (child.button == button)
			{
				return root;
			}

			MenuItem found = FindParentFrom(child, button);
			if (found != null)
			{
				return found;
			}
		}

		return null;
	}
}
