/// <summary>
/// Interface for grouping tweens
/// </summary>
public interface ITweenGroup
{
	void Init ();

	ITweenGroup GetGroup (string key);

	void Play (string key, bool forward = true);

	void PlayForward ();

	void PlayReverse ();

	void OnFinished (bool isLast);

	void Reset (string key);

	void Reset (int index);
}
