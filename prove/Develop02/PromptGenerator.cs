using System;
using System.Collections.Generic;
using System.Linq;

namespace Develop02
{
    /// <summary>
    /// Provides random prompts to help the user start their journal entry.
    /// </summary>
    public class PromptGenerator
    {
        private readonly List<string> _prompts;
        private readonly Random _random;

        public PromptGenerator(IEnumerable<string> prompts)
        {
            if (prompts == null)
            {
                throw new ArgumentNullException(nameof(prompts));
            }

            _prompts = prompts.Where(prompt => !string.IsNullOrWhiteSpace(prompt)).Distinct().ToList();
            if (_prompts.Count == 0)
            {
                throw new ArgumentException("Prompt list must contain at least one prompt.", nameof(prompts));
            }

            _random = new Random();
        }

        public string GetRandomPrompt()
        {
            var index = _random.Next(_prompts.Count);
            return _prompts[index];
        }
    }
}
