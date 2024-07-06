using System.Collections.Generic;
using Godot;

namespace RA2Survivors
{
    public abstract partial class Weapon : Node
    {
        // how much each bursts deals
        public double damageMultiplier;

        // how many bursts in a shot
        public int burstCount;

        // how long between each burst
        public double burstDelay;

        // howlong it takes to reload
        public double reloadSpeed;

        public int multishot;

        protected double _burstTimeout;
        protected double _reloadTimeout;
        protected int _burstsLeft;
        protected List<Enemy> _selectedTargets;

        public Player owner;

        protected EWeaponState currentState = EWeaponState.Shooting;

        public override void _Ready()
        {
            owner = GetParent<Player>();
        }

        public void Reload()
        {
            _reloadTimeout *= 0.9;
        }

        public override void _Process(double delta)
        {
            switch (currentState)
            {
                case EWeaponState.Shooting:
                    ProcessShooting(delta);
                    break;
                case EWeaponState.Reloading:
                    ProcessReloading(delta);
                    break;
            }
        }

        public void ProcessShooting(double delta)
        {
            if (_burstsLeft > 0)
            {
                _burstTimeout -= delta;
                if (_burstTimeout <= 0)
                {
                    // check that all references in _selectedTargets are still valid
                    for (int i = _selectedTargets.Count - 1; i >= 0; i--)
                    {
                        if (!IsInstanceValid(_selectedTargets[i]))
                        {
                            _selectedTargets.RemoveAt(i);
                        }
                    }
                    Shoot(_selectedTargets);
                    _burstTimeout = burstDelay / owner.stats.attackSpeed;
                    _burstsLeft--;
                }
            }
            else
            {
                currentState = EWeaponState.Reloading;
            }
        }

        public void ProcessReloading(double delta)
        {
            _reloadTimeout -= delta;
            if (_reloadTimeout <= 0)
            {
                _reloadTimeout = reloadSpeed / owner.stats.attackSpeed;
                _burstsLeft = burstCount;
                _selectedTargets = GetTargets();
                currentState = EWeaponState.Shooting;
            }
        }

        public abstract List<Enemy> GetTargets();

        public abstract void Shoot(List<Enemy> targets);
    }
}