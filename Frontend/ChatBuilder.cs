using System;
using System.Collections.Generic;
using System.Text;

namespace Frontend
{
    public sealed class ChatBuilder
    {
        private Chat _workingCopy = new Chat();
        private StringBuilder _text = new StringBuilder();
        private List<Chat> _extra = new List<Chat>();
        
        public Chat Build()
        {
            _workingCopy.Text = _text.ToString();
            if (_extra.Count > 0)
                _workingCopy.Extra = _extra.ToArray();

            return _workingCopy;
        }

        public ChatBuilder WithColor(string color)
        {
            _workingCopy.Color = color;
            return this;
        }
        
        public ChatBuilder Underlined()
        {
            _workingCopy.Underlined = true;
            return this;
        }
        
        public ChatBuilder Strikethrough()
        {
            _workingCopy.Strikethrough = true;
            return this;
        }
        
        public ChatBuilder Bold()
        {
            _workingCopy.Bold = true;
            return this;
        }
        
        public ChatBuilder Obfuscated()
        {
            _workingCopy.Obfuscated = true;
            return this;
        }
        
        public ChatBuilder Italic()
        {
            _workingCopy.Italic = true;
            return this;
        }

        public ChatBuilder AppendText(string text)
        {
            _text.Append(text);
            return this;
        }

        public ChatBuilder WithExtra(Action<ChatBuilder> builder)
        {
            var b = new ChatBuilder();
            builder(b);
            _extra.Add(b.Build());
            return this;
        }

        public ChatBuilder WithInsertion(string insertion)
        {
            _workingCopy.Insertion = insertion;
            return this;
        }
    }
}