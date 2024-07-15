using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

//丧尸
public class Enemy1 : Enemy
{

    public static Enemy1 Create(Vector3 position)
    {
        GameObject enemyPrefab=Resources.Load<GameObject>("FightObject/Character/Enemy1");
        GameObject enemyObject=GameObject.Instantiate(enemyPrefab);
        enemyObject.transform.position=position;
        Enemy1 enemy=enemyObject.AddComponent<Enemy1>();
        PropertySheet propertySheet=CharacterUtility.GetBasePropertySheet("Enemy1",1);
        enemy.Init(propertySheet);
        return enemy;
    }
    public override void Move()
    {
        base.Move();
        Vector3 playerPosition=Player.GetCurrent().transform.position;
        Vector3 enemyPosition=transform.position;
        Vector3 direction=playerPosition-enemyPosition;
        direction.z=0;
        float distance=direction.sqrMagnitude;
        if(distance>1)
        {   
            transform.localRotation=Quaternion.LookRotation(Vector3.forward, direction);
            transform.position+=direction.normalized*mCurrentPropertySheet.GetMoveSpeed()*Time.deltaTime;
            //Debug.Log("移动速度:"+mCurrentPropertySheet.GetMoveSpeed());
        }
    }
    protected void Init(PropertySheet basePropertySheet)
    {
        base.Init(CharacterId.Enemy1,basePropertySheet);
        CanShootFlag=false;
        CanShootFlag=true;
    }
    public override void OnUpdate()
    {
        base.OnUpdate();
        Debug.Log("移动速度:"+mCurrentPropertySheet.GetMoveSpeed());
        Move();
    }
}
