using GameLib.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameLib.Services
{
    public interface IGameBoardFactory
    {
        GameBoard Create();
    }
}
