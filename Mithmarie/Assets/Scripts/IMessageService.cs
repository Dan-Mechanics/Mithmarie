using UnityEngine;

namespace Mitholca
{
    public interface IMessageService 
    {
        void Send(string text, Color color);
    }
}