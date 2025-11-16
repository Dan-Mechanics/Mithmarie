using UnityEngine;

namespace Mithmarie
{
    public interface IMessageService 
    {
        void Send(string text, Color color);
    }
}