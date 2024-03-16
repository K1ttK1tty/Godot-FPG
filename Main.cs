using Godot;
using System;


public interface Repairable // Ремонтируемый
{
    public float Wear { get; } // Износ (якобы процент)
    public float GetRepairCost(); // Получить стоимость ремонта
    public void Repair(); // Отремонтировать
}
public interface FillableWithFuel // Заправляемый топливом
{
    public float FuelLevel { get; } // Уровень топлива
    public float CapacityOfFuelTank { get; } // Объём топливного бака
    public void FillWithFuel(); // Заправить топливом
}

class Building : Repairable // Ремонтируемый
{
    public float Wear => 26.0f; // износ
    public float GetRepairCost()
    {
        return 100.00f;
    }
    public void Repair() // Отремонтировать
    {
        // Ремонт
    }

    public string Adress => ""; // Адрес
    public float Square => 30; // Площадь
    public DateTime DateOfBuild => new(2015, 7, 20); //  Дата Постройки
    public string GetData => ""; // Получить данные
}

class Class : FillableWithFuel //
{
    public float FuelLevel => 88.5f; // Уровень топлива
    public float CapacityOfFuelTank => 46f; // Объем топливного бака

    public void FillWithFuel() // Заправить топливом
    {
        // Заправить
    }

    public string RegistrationNumber => "C227HA"; // Регистрационынй номер
    public string CarBrand => "Hyundai"; // Марка авто
    public DateTime DateOfRelease => new(2010, 2, 15); // Дата выпуска
    public float Mileage => 189634f; // Пробег
    public string getData() // Получить данные
    {
        return "";
    }
}



public partial class Main : Node
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }
}
