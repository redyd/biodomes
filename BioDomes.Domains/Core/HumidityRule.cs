namespace BioDomes.Domains.Core;

public class HumidityRule(double temperature, double humidity)
{
    public bool IsValid => humidity <= MaxHumidity;

    public double MaxHumidity =>
        216.7 * 6.112 * Math.Exp(17.67 * temperature / (temperature + 243.5)) / (temperature + 273.15);

    public double HumidityPercent => Math.Min(MaxHumidity > 0 ? humidity / MaxHumidity * 100 : 0, 100);
}