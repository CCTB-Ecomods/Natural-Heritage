using System;

[Serializable]
public class Bonus
{
    private int _duration;
    private string _name;
    private Func<bool> _test;
    private float _value;

    /**
	 * Default constructor: just takes a bonus value that is valid for one turn.
	 */
    public Bonus(string name, float value)
    {
        _name = name;
        _value = value;
        _duration = 1;
        _test = null;
    }

    /**
	 * Duration constructor: set the bonus value and for how long it's valid.
	 */
    public Bonus(string name, float value, int duration)
    {
        _name = name;
        _value = value;
        _duration = duration;
        _test = null;
    }

    /**
	 * Custom constructor: define the bonus value and pass a custom function to test its validity.
	 */
    public Bonus(string name, float value, Func<bool> test)
    {
        _name = name;
        _value = value;
        _duration = -1;
        _test = test;
    }

    /**
	 * Check whether the bonus is still valid.
	 */
    public bool IsValid()
    {
        if (_duration == 0) return false;
        if (_duration > 0) _duration--;
        if (_test != null) return _test();
        return true;
    }

    public string GetName()
    {
        return _name;
    }

    public float GetValue()
    {
        return _value;
    }
}