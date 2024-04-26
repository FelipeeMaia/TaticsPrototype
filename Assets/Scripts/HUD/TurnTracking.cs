using System.Collections.Generic;
using System.Linq;
using Tactics.Core;
using Tactics.Managment;
using UnityEngine;

namespace Tactics.HUD
{
    public class TurnTracking : MonoBehaviour
    {
        [SerializeField] List<TurnCard> _cardsOn;
        [SerializeField] List<TurnCard> _cardsOff;
        [SerializeField] TurnManager _turnManager;
        [SerializeField] GameManager _gameManager;

        void Awake()
        {
            _turnManager.OnTurnStacked += AddCard;
            _turnManager.OnTurnEnd += RemoveTopCard;
            _gameManager.OnUnitSpawn += PrepareDeath;
        }


        public void AddCard(Unit unit)
        {
            var card = _cardsOff[0];

            _cardsOff.Remove(card);
            _cardsOn.Add(card);

            int index = _cardsOn.IndexOf(card);
            card.Link(unit, index);
        }

        public void RemoveTopCard()
        {
            var card = _cardsOn[0];
            float waitTime = card.lerpDuration;

            TurnCardOff(card);

            Invoke("UpdateCardsPosition", 0.5f);
        }
        
        public void PrepareDeath(Unit unit)
        {
            unit.health.OnDeath += RemoveDeadCards;
        }

        public void RemoveDeadCards(Unit deadUnit)
        {
            var deadCards = new List<TurnCard>();

            foreach(var card in _cardsOn)
            {
                if(card.unit == deadUnit)
                {
                    deadCards.Add(card);
                }
            }

            foreach(var card in deadCards)
            {
                TurnCardOff(card);
            }

            UpdateCardsPosition();
            deadUnit.health.OnDeath -= RemoveDeadCards;
        }

        private void TurnCardOff(TurnCard card)
        {
            _cardsOff.Add(card);
            _cardsOn.Remove(card);
            card.Unlink();
        }

        private void UpdateCardsPosition()
        {
            for(int i = 0; i < _cardsOn.Count; i++)
            {
                _cardsOn[i].ChangePosition(i);
            }
        }
    }
}