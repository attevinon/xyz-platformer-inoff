using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroInputReader : MonoBehaviour
{
    [SerializeField] private HeroScript _hero;

    private void Awake()
    {
        _hero = GetComponent<HeroScript>();
    }
    void Update()
    {
        ///<summary>
        ///Работает, но движение идёт рывками и нет движения по диагонали
        ///Раскомментить чтобы протестировать
        ///</summary>
        /*GetKeyMovementInput();
        GetKeyAttackInput();*/

        LegacyAxesInput();
        LegacyAttackInput();
    }

    #region LegacyVirtualInput
    private void LegacyAttackInput()
    {
        if (Input.GetButtonUp("Attack"))
        {
            _hero.Attack();
        }
    }

    private void LegacyAxesInput()
    {
        var _horizontal = Input.GetAxis("Horizontal");
        var _vertical = Input.GetAxis("Vertical");
        _hero.SetDirection(_horizontal, _vertical);
    }
    #endregion

    #region KeyboardManualInput
    private void GetKeyMovementInput()
    {
        if (Input.GetKey(KeyCode.D))
        {
            _hero.SetDirection(1, 0);
        } 
        else if (Input.GetKey(KeyCode.A))
        {
            _hero.SetDirection(-1, 0);
        }
        else if (Input.GetKey(KeyCode.W))
        {
            _hero.SetDirection(0, 1);

        }
        else if (Input.GetKey(KeyCode.S))
        {
            _hero.SetDirection(0, -1);

        }
        else
        {
            _hero.SetDirection(0, 0);
        }
    }

    private void GetKeyAttackInput()
    {
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            _hero.Attack();
        }
    }
    #endregion
}
