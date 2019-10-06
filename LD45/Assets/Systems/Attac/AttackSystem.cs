using System;
using System.Linq;
using SystemBase;
using Systems.Attac.Actions;
using Systems.InputHandling;
using Systems.Nomes;
using Systems.Player;
using UniRx;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Systems.Attac
{
    [GameSystem(typeof(PlayerSystem))]
    public class AttackSystem : GameSystem<AttackComponent>
    {
        private AttackComponent _attacks;

        public override void Register(AttackComponent component)
        {
            _attacks = component;

            MessageBroker.Default.Receive<ActAttackSpawn>()
                .Subscribe(SpawnHitObject)
                .AddTo(component);
        }

        private void SpawnHitObject(ActAttackSpawn attcSpwMsg)
        {
            GameObject attackObject;
            switch (attcSpwMsg.Word)
            {
                case InputWordType.Fire:
                    attackObject = Object.Instantiate(_attacks.AttackPrefabs.First(definition => definition.WordType == attcSpwMsg.Word).Prefab,
                        new Vector3(attcSpwMsg.Position.x, 1, attcSpwMsg.Position.y),
                        Quaternion.Euler(attcSpwMsg.Direction.x, 0, attcSpwMsg.Direction.y));
                    attackObject.GetComponent<FireComponent>().Originator = attcSpwMsg.Originator;
                    attackObject.transform.SetParent(attcSpwMsg.Originator.transform);
                    break;

                case InputWordType.Hit:
                    attackObject = Object.Instantiate(_attacks.AttackPrefabs.First(definition => definition.WordType == attcSpwMsg.Word).Prefab,
                        new Vector3(attcSpwMsg.Position.x, 1, attcSpwMsg.Position.y),
                        Quaternion.Euler(attcSpwMsg.Direction.x, 0, attcSpwMsg.Direction.y));
                    attackObject.GetComponent<HitComponent>().Originator = attcSpwMsg.Originator;
                    attackObject.transform.SetParent(attcSpwMsg.Originator.transform);
                    break;

                case InputWordType.Key:
                    attackObject = Object.Instantiate(_attacks.AttackPrefabs.First(definition => definition.WordType == attcSpwMsg.Word).Prefab,
                        new Vector3(attcSpwMsg.Position.x, 0, attcSpwMsg.Position.y),
                        Quaternion.Euler(attcSpwMsg.Direction.x, 0, attcSpwMsg.Direction.y));
                    attackObject.GetComponent<KeyComponent>().Originator = attcSpwMsg.Originator;
                    attackObject.transform.SetParent(attcSpwMsg.Originator.transform);
                    break;

                case InputWordType.Megahit:
                    attackObject = Object.Instantiate(_attacks.AttackPrefabs.First(definition => definition.WordType == attcSpwMsg.Word).Prefab,
                        new Vector3(attcSpwMsg.Position.x, 1, attcSpwMsg.Position.y),
                        Quaternion.Euler(attcSpwMsg.Direction.x, 0, attcSpwMsg.Direction.y));
                    attackObject.GetComponent<MegaHitComponent>().Originator = attcSpwMsg.Originator;
                    attackObject.transform.SetParent(attcSpwMsg.Originator.transform);
                    break;

                case InputWordType.Magic:
                    attackObject = Object.Instantiate(_attacks.AttackPrefabs.First(definition => definition.WordType == attcSpwMsg.Word).Prefab,
                        new Vector3(attcSpwMsg.Position.x, 1, attcSpwMsg.Position.y),
                        Quaternion.Euler(attcSpwMsg.Direction.x, 0, attcSpwMsg.Direction.y));
                    attackObject.GetComponent<MagicComponent>().Originator = attcSpwMsg.Originator;
                    attackObject.transform.SetParent(attcSpwMsg.Originator.transform);
                    break;

                case InputWordType.Nothing:
                    break;

                case InputWordType.Parry:
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}