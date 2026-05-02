namespace BioDomes.Domains.Core;

public class HumidityRule
{
    private readonly double _temp;
    private readonly double _hum;

    public HumidityRule(double temperature, double humidity)
    {
        _temp = temperature;
        _hum = humidity;
    }

    public bool IsValid => _hum <= MaxHumidity;

    public double MaxHumidity =>
        216.7 * 6.112 * Math.Exp(17.67 * _temp / (_temp + 243.5)) / (_temp + 273.15);

    public double HumidityPercent => Math.Min(MaxHumidity > 0 ? _hum / MaxHumidity * 100 : 0, 100);
}