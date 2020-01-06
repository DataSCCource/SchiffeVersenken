using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchiffeVersenken
{
    public interface IUpdateUI
    {
        void UpdateField();
        void UpdateScore();
        void ShowHitMessage();
        void PlayerWonMessage();
    }
}
