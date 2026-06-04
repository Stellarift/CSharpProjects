using System;
using System.Runtime.InteropServices;

namespace MonitorDiagonal;

class Program
{
    // Подключаем нативную библиотеку macOS для работы с дисплеем
    [DllImport("/System/Library/Frameworks/CoreGraphics.framework/CoreGraphics")]
    private static extern nint CGMainDisplayID();

    [DllImport("/System/Library/Frameworks/CoreGraphics.framework/CoreGraphics")]
    private static extern float CGDisplayScreenSize(nint displayID);

    // Альтернативная функция для получения информации о дисплее
    [DllImport("/System/Library/Frameworks/CoreGraphics.framework/CoreGraphics")]
    private static extern nint CGDisplayCopyDisplayMode(nint displayID);

    [DllImport("/System/Library/Frameworks/CoreGraphics.framework/CoreGraphics")]
    private static extern double CGDisplayModeGetWidth(nint mode);

    [DllImport("/System/Library/Frameworks/CoreGraphics.framework/CoreGraphics")]
    private static extern double CGDisplayModeGetHeight(nint mode);

    [DllImport("/System/Library/Frameworks/CoreGraphics.framework/CoreGraphics")]
    private static extern void CGDisplayModeRelease(nint mode);

    static void Main(string[] args)
    {
        try
        {
            Console.WriteLine("Определение диагонали монитора (macOS)\n");

            // Получаем ID основного дисплея
            nint mainDisplay = CGMainDisplayID();
            
            // Получаем физический размер диагонали в миллиметрах
            float diagonalMM = CGDisplayScreenSize(mainDisplay);
            
            // Получаем разрешение экрана для дополнительной информации
            nint displayMode = CGDisplayCopyDisplayMode(mainDisplay);
            double widthPx = CGDisplayModeGetWidth(displayMode);
            double heightPx = CGDisplayModeGetHeight(displayMode);
            CGDisplayModeRelease(displayMode);
            
            // Переводим миллиметры в дюймы (1 дюйм = 25.4 мм)
            double diagonalInches = diagonalMM / 25.4;
            
            // Выводим информацию
            Console.WriteLine("Информация об основном мониторе:\n");
            Console.WriteLine($"   Разрешение: {widthPx:F0} x {heightPx:F0} пикселей");
            Console.WriteLine($"   Физический размер диагонали: {diagonalMM:F0} мм");
            Console.WriteLine($"   Диагональ в дюймах: {diagonalInches:F1}\"");
            
            // Дополнительная информация о точности
            Console.WriteLine($"\nФормула расчёта:");
            Console.WriteLine($"   diagonalInches = diagonalMM / 25.4");
            Console.WriteLine($"   {diagonalMM:F0} / 25.4 = {diagonalInches:F1}");
            
            Console.WriteLine("\nНажмите любую клавишу для выхода...");
            Console.ReadKey();
        }
        catch (DllNotFoundException ex)
        {
            Console.WriteLine($"Не удалось загрузить библиотеку CoreGraphics");
            Console.WriteLine($"   {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }
}