using System;
using System.Collections.Generic;

namespace AyaGameEngine2D
{
    /// <summary>
    /// 类      名：ClientEvent
    /// 功      能：客户端事件
    /// 说      明：客户端事件队列的子元素，包含消息号，消息参数，和消息回调。
    /// 日      期：2016-01-30
    /// 修      改：2016-01-30
    /// 作      者：ls9512
    /// </summary>
    internal class ClientEvent
    {
        #region 公有成员
        /// <summary>
        /// 消息号
        /// </summary>
        public int MsgIndex { get; private set; }
        /// <summary>
        /// 参数
        /// </summary>
        public object Arg { get; private set; }
        /// <summary>
        /// 回调
        /// </summary>
        public Action<object> CallBack { get; private set; }
        #endregion

        #region 构造方法
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="index">消息号</param>
        /// <param name="arg">参数</param>
        /// <param name="callback">回调函数</param>
        public ClientEvent(int msgIndex, object arg, Action<object> callback)
        {
            Arg = arg;
            CallBack = callback;
            MsgIndex = msgIndex;
        }
        #endregion

        #region 公有方法
        /// <summary>
        /// 执行回调
        /// </summary>
        public void DealCallBack()
        {
            if (CallBack != null)
            {
                CallBack(Arg);
            }
        } 
        #endregion
    }

    /// <summary>
    /// 类      名：ClientEventHandler
    /// 功      能：客户端消息队列处理类
    /// 说      明：接收消息并存入队列，通过消息号保证消息严格按顺序执行。
    ///             每次PushEvent(...)，都会将队列中现有的消息全部处理，直到消息号不连续。
    /// 日      期：2016-01-30
    /// 修      改：2016-01-30
    /// 作      者：ls9512
    /// </summary>
    public class ClientEventHandler
    {
        #region 公有成员
        /// <summary>
        /// 消息号
        /// </summary>
        public int Index { get; private set; }
        /// <summary>
        /// 错误回调
        /// </summary>
        public Action<object> FailCallBack { get; } 
        #endregion

        #region 私有成员
        /// <summary>
        /// 事件队列
        /// </summary>
        private readonly List<ClientEvent> _eventList = new List<ClientEvent>(); 
        #endregion

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="failBack">错误回调</param>
        public ClientEventHandler(Action<object> failBack = null)
        {
            Index = 0;
            FailCallBack = failBack;
        }
        #endregion

        #region 公有方法
        /// <summary>
        /// 存入消息
        /// </summary>
        /// <param name="msgIndex">消息号</param>
        /// <param name="arg">参数</param>
        /// <param name="callBack">回调</param>
        public void PushEvent(int msgIndex, object arg, Action<object> callBack)
        {
            ClientEvent cEvent = new ClientEvent(msgIndex, arg, callBack);
            if (cEvent.MsgIndex == -1 || cEvent.MsgIndex < Index)
            {
                // 消息队列错误处理
                if (FailCallBack != null)
                {
                    FailCallBack(arg);
                    Debug.ThrowException("事件序列错误", new Exception("事件序号:" + msgIndex));
                }
            }
            else
            {
                // 添加事件到消息队列并排序
                _eventList.Add(cEvent);
                sortEventList();
            }
            // 处理消息
            handleEvent();
        }
        #endregion

        #region 私有方法
        /// <summary>
        /// 处理消息
        /// </summary>
        private void handleEvent()
        {
            ClientEvent cEvent;
            while ((cEvent = popEvent(Index)) != null)
            {
                cEvent.DealCallBack();
                ++Index;
            }
        }

        /// <summary>
        /// 取出消息
        /// </summary>
        /// <param name="eventIndex">消息号</param>
        /// <returns>结果</returns>
        private ClientEvent popEvent(int eventIndex)
        {
            int listCount = _eventList.Count;
            if (listCount <= 0)
            {
                return null;
            }
            if (_eventList[listCount - 1].MsgIndex == eventIndex)
            {
                ClientEvent cEvent = _eventList[listCount - 1];
                _eventList.RemoveAt(listCount - 1);
                return cEvent;
            }
            return null;
        }

        /// <summary>
        /// 消息排序
        /// </summary>
        private void sortEventList()
        {
            _eventList.Sort((lhs, rhs) =>
            {
                if (lhs == rhs)
                {
                    return 0;
                }
                else
                {
                    return lhs.MsgIndex < rhs.MsgIndex ? 1 : -1;
                }
            });
        }

        /// <summary>
        /// 清除消息
        /// </summary>
        public void Clear()
        {
            Index = 0;
            _eventList.Clear();
        } 
        #endregion

        #region 析构方法
        /// <summary>
        /// 析构函数
        /// </summary>
        ~ClientEventHandler()
        {
            Clear();
        } 
        #endregion
    }
}
