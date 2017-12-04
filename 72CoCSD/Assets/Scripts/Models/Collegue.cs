using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Assets.Scripts.Managers;
using Assets.Scripts.UI;

namespace Assets.Scripts.Models
{
    [Serializable]
    public class Collegue : ContactBase
    {
        public string DefaultName { get; set; }
        
        public List<Dialog> Dialogs;

        private Dialog CurrentDialog;

        private Issue SimpliestIssue;

        public override float Read(string playerText)
        {
            if (CurrentDialog.CurrentLine.AckAsLowerComplexityIssue)
            {
                return playerText.ToLower() == SimpliestIssue.Answer.ToString() ? 1f : 0f;
            }

            return 1f;
        }

        public override ChatLine Speak()
        {
            CurrentDialog = Dialogs.FirstOrDefault();

            if (CurrentDialog == null || !CurrentDialog.MoveNext())
            {
                Dialogs.RemoveAt(0);
                return null;
            }
            
            NextForcedPlayerInput = CurrentDialog.CurrentLine.ForcedAnswer;

            if (CurrentDialog.CurrentLine.AckAsLowerComplexityIssue)
            {
                SimpliestIssue = GameManager.Instance.Game.Issues.Where(i=> i.Unlocked).OrderBy(i => i.Complexity).First();
            }

            return new ChatLine
            {
                Author = CurrentDialog.CurrentLine.OverrideSpeaker ?? Name,
                Text = CurrentDialog.CurrentLine.AckAsLowerComplexityIssue
                    ? SimpliestIssue.Question.ToString()
                    : CurrentDialog.CurrentLine.Question
            };
        }

        public override ChatLine GetLastSentence()
        {
            return
                CurrentDialog == null || CurrentDialog.CurrentLine == null
                    ? null
                    : new ChatLine
                    {
                        Author = CurrentDialog.CurrentLine.OverrideSpeaker ?? Name,
                        Text = CurrentDialog.CurrentLine.AckAsLowerComplexityIssue
                            ? SimpliestIssue.Question.ToString()
                            : CurrentDialog.CurrentLine.Question
                    };
        }

        public override void QuitSatified()
        {
            GameManager.Instance.Game.PauseHandle--;
            base.QuitSatified();
        }
    }
}