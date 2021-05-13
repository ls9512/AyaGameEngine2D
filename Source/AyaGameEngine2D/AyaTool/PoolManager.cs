using System.Collections.Generic;

namespace AyaGameEngine2D
{
    /// <summary>
    /// 类      名：IPoolObject
    /// 功      能：对象池管理接口，对象需实现该接口已获得对象池管理器的使用支持。
    /// 日      期：2016-01-03
    /// 修      改：2016-01-03
    /// 作      者：ls9512
    /// </summary>
    public interface IPoolObject
    {
        /// <summary>
        /// 重置方法，用于对象被存入池中后恢复初始设置。
        /// </summary>
        void Reset();
    }

    /// <summary>
    /// 类      名：PoolManager
    /// 功      能：对象池管理器
    /// 说      明：适用于存取频繁创建、销毁的对象管理(如粒子系统)，以减小实例化的性能开销。
    ///             使用时，先初始化该对象池，然后每次创建时调用Create()，用完时调用Store()。存入的对象会在再次取出时自动调用Reset()恢复初始设置。
    /// 日      期：2016-01-03
    /// 修      改：2016-01-03
    /// 作      者：ls9512
    /// </summary>
    /// <typeparam name="T">被管理对象类型</typeparam>
    public class PoolManager<T> where T : class, IPoolObject, new()
    {
        #region 委托回调
        /// <summary>
        /// 对象池重置委托
        /// </summary>
        public delegate void PoolResetEventHandler();
        /// <summary>
        /// 对象池初始化委托
        /// </summary>
        public delegate void PoolInitEventHandler();
        /// <summary>
        /// 重置操作委托
        /// </summary>
        private readonly PoolResetEventHandler _onPoolReset;
        /// <summary>
        /// 初始化操作委托
        /// </summary>
        private readonly PoolInitEventHandler _onPoolInit;  
        #endregion

        #region 私有成员
        /// <summary>
        /// 对象栈
        /// </summary>
        private readonly Stack<T> _objectStack;
        #endregion

        #region 构造方法
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="maxSize">最大尺寸</param>
        /// <param name="resetAction">重置操作委托(可空)</param>
        /// <param name="initAction">初始化操作委托(可空)</param>
        public PoolManager(int maxSize, PoolResetEventHandler resetAction = null, PoolInitEventHandler initAction = null)
        {
            _objectStack = new Stack<T>(maxSize);
            _onPoolReset = resetAction;
            _onPoolInit = initAction;
        }
        #endregion

        #region 存取方法
        /// <summary>
        /// 创建对象
        /// 如果池中有，则直接取出；如果没有，则创建。
        /// </summary>
        /// <returns>创建结果</returns>
        public T Create()
        {
            if (_objectStack.Count > 0)
            {
                T t = _objectStack.Pop();
                t.Reset();
                if (_onPoolReset != null) _onPoolReset();
                return t;
            }
            else
            {
                T t = new T();
                if (_onPoolInit != null) _onPoolInit();
                return t;
            }
        }

        /// <summary>
        /// 存储对象
        /// </summary>
        /// <param name="obj">对象</param>
        public void Store(T obj)
        {
            _objectStack.Push(obj);
        }
        #endregion
    }
}
