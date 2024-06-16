using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChessPiece : MonoBehaviour, IEntity
{
    [Header("Stats")]
    [SerializeField] private float fullHealth;
    [SerializeField] private float attackStreght;
    [SerializeField] private float deathDelay;

    [SerializeField] private Image healthBar;
    [SerializeField] private Image attackImage;
    [SerializeField] private TMP_Text damageText;

    public bool IsRetreating;
    public Action OnRetreating;

    private int _retreatingCount;
    private float _currentHealth;
    private Action onGettingDamage;
    private Player _player;
    private bool _isKilled;

    public void Create()
    {
        onGettingDamage += Kill;

        _currentHealth = fullHealth;
        healthBar.fillAmount = 1;
        _isKilled = false;
        _retreatingCount = 0;
    }
    public void SetPlayer(Player player)
    {
        _player = player;
    }

    public void Retreat()
    {
        if (_retreatingCount < 3)
        {
            _retreatingCount++;
            IsRetreating = true;
        }
        else
        {
            _retreatingCount = 0;
            IsRetreating = false;
            Attack(_player);
        }
        Debug.LogError("run");
    }

    public void Kill()
    {
        StartCoroutine(DeathCooldown());
    }
    private IEnumerator DeathCooldown()
    {
        yield return new WaitForSeconds(deathDelay);

        _player.RestoreHealth(_player.FullHealth);
        _isKilled = true;
    }

    public EntityController Spawn(ChessBoardCell cell)
    {
        var chesspieceRotation = new Quaternion(0, 180, 0, 0);

        var entity = Instantiate(gameObject, cell.transform.position, chesspieceRotation);

        return entity.GetComponent<ChessPieceController>();
    }

    public void Attack(IEntity player)
    {
        IsRetreating = false;
        attackImage.gameObject.SetActive(true);
        player.GetDamage(attackStreght);

        StartCoroutine(SetActiveCooldown(attackImage.gameObject));
    }
    private IEnumerator SetActiveCooldown(GameObject obj)
    {
        yield return new WaitForSeconds(1f); 
        obj.SetActive(false);
    }

    public void GetDamage(float damage)
    {
        _currentHealth -= damage;

        damageText.text = "-" + damage;
        damageText.gameObject.SetActive(true);
        StartCoroutine(SetActiveCooldown(damageText.gameObject));

        Debug.LogError(_currentHealth / fullHealth);
        healthBar.fillAmount = (_currentHealth / fullHealth) * 1f;

        if (_currentHealth <= 0) onGettingDamage?.Invoke();
    }

    public float GetAfterAttackHealth(float damage)
    {
        return _currentHealth - damage;
    }
    public float GetStrength()
    {
        return attackStreght;
    }
    public float GetCurrentHealth()
    {
        return _currentHealth;
    }

    public bool IsKilled()
    {
        return _isKilled;
    }
}
