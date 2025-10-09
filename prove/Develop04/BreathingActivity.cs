using System;

public class BreathingActivity : MindfulnessActivity
{
    public BreathingActivity()
        : base(
            "Breathing Activity",
            "This activity will help you relax by walking you through breathing in and out slowly. Clear your mind and focus on your breathing.")
    {
    }

    protected override void PerformActivity()
    {
        int elapsed = 0;

        while (elapsed < Duration)
        {
            Console.Write("Breathe in... ");
            int breatheIn = Math.Min(4, Duration - elapsed);
            ShowCountdown(breatheIn);
            elapsed += breatheIn;

            if (elapsed >= Duration)
            {
                break;
            }

            Console.Write("Breathe out... ");
            int breatheOut = Math.Min(4, Duration - elapsed);
            ShowCountdown(breatheOut);
            elapsed += breatheOut;
        }

        Console.WriteLine();
    }
}
