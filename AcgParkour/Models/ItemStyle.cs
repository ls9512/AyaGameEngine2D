﻿using System;
using System.Collections.Generic;
using System.Text;

namespace AcgParkour.Models
{
    /// <summary>
    /// 类      名：ItemStyle
    /// 功      能：物品摆放样式静态类
    /// 日      期：2015-03-21
    /// 修      改：2015-03-21
    /// 作      者：ls9512
    /// </summary>
    public static class ItemStyle
    {
        /// <summary>
        /// 摆放样式表
        /// 0 无物件 1 有物件
        /// </summary>
        public static int[, ,] StyleGrid = {
                                        {{0,0,0,0,0,0,0,0,0},
                                        {0,0,0,0,0,0,0,0,0},
                                        {0,0,0,0,0,0,0,0,0},
                                        {0,0,0,0,0,0,0,0,0},
                                        {0,0,0,0,0,0,0,0,0},
                                        {0,0,1,1,1,1,1,0,0},
                                        {0,1,1,1,1,1,1,1,0},
                                        {1,1,0,0,0,0,0,1,1},
                                        {1,0,0,0,0,0,0,0,1}
                                        },
                                        {{0,0,0,0,0,0,0,0,0},
                                        {0,0,0,0,0,0,0,0,0},
                                        {0,0,0,0,0,0,0,0,0},
                                        {0,0,0,0,0,0,0,0,0},
                                        {0,0,0,0,0,0,0,0,0},
                                        {0,0,0,0,0,0,0,0,0},
                                        {0,0,0,0,0,0,0,0,0},
                                        {0,1,0,1,0,1,0,1,0},
                                        {1,0,1,0,1,0,1,0,1}
                                        },
                                        {{0,0,0,0,0,0,0,0,0},
                                        {0,0,0,0,0,0,0,0,0},
                                        {0,0,0,0,0,0,0,0,0},
                                        {0,0,0,0,0,0,0,0,0},
                                        {0,0,0,0,0,0,0,0,0},
                                        {0,0,0,0,0,0,0,0,0},
                                        {0,0,0,0,0,0,0,0,0},
                                        {0,1,1,1,1,1,1,1,0},
                                        {1,1,1,1,1,1,1,1,1}
                                        },
                                        {{0,0,0,0,0,0,0,0,0},
                                        {0,0,0,0,0,0,0,0,0},
                                        {0,0,0,0,0,0,0,0,0},
                                        {0,0,0,0,0,0,0,0,0},
                                        {0,0,0,0,0,0,0,0,0},
                                        {0,0,0,0,0,0,0,0,0},
                                        {0,0,0,0,0,0,0,0,0},
                                        {0,0,0,0,0,0,0,0,0},
                                        {1,1,1,1,1,1,1,1,1}
                                        },
                                        {{0,0,0,0,0,0,0,0,0},
                                        {0,1,1,0,0,0,1,1,0},
                                        {1,0,0,1,0,1,0,0,1},
                                        {1,0,0,0,1,0,0,0,1},
                                        {1,0,0,0,0,0,0,0,1},
                                        {0,1,0,0,0,0,0,1,0},
                                        {0,0,1,0,0,0,1,0,0},
                                        {0,0,0,1,0,1,0,0,0},
                                        {0,0,0,0,1,0,0,0,0}
                                        },
                                        {{0,0,0,0,0,0,0,0,0},
                                        {0,0,0,0,0,0,0,0,0},
                                        {0,0,0,0,0,0,0,0,0},
                                        {0,0,0,0,0,0,0,0,0},
                                        {0,0,0,0,1,0,0,0,0},
                                        {0,0,0,1,0,1,0,0,0},
                                        {0,0,1,0,0,0,1,0,0},
                                        {0,1,0,0,0,0,0,1,0},
                                        {1,1,1,1,1,1,1,1,1}
                                        },
                                        {{0,0,0,0,0,0,0,0,0},
                                        {0,0,0,0,0,0,0,0,0},
                                        {0,0,0,0,0,0,0,0,0},
                                        {0,0,0,0,0,0,0,0,0},
                                        {0,0,0,1,1,1,0,0,0},
                                        {0,0,1,1,1,1,1,0,0},
                                        {0,1,1,0,0,0,1,1,0},
                                        {1,1,0,0,0,0,0,1,1},
                                        {1,1,0,0,0,0,0,1,1}
                                        },
                                        {{0,0,0,0,0,0,0,0,0},
                                        {0,0,0,0,0,0,0,0,0},
                                        {0,0,0,0,0,0,0,0,0},
                                        {0,0,0,0,0,0,0,0,0},
                                        {1,1,1,1,1,1,1,1,1},
                                        {1,1,1,1,1,1,1,1,1},
                                        {0,0,0,0,0,0,0,0,0},
                                        {0,0,0,0,0,0,0,0,0},
                                        {0,0,0,0,0,0,0,0,0}
                                        },
                                        {{0,0,0,0,0,0,0,0,0},
                                        {0,0,0,0,0,0,0,0,0},
                                        {0,0,0,0,0,0,0,0,0},
                                        {0,0,0,0,0,0,0,0,0},
                                        {1,1,0,0,1,0,0,1,1},
                                        {1,1,0,0,1,0,0,1,1},
                                        {0,0,0,0,0,0,0,0,0},
                                        {0,0,0,0,0,0,0,0,0},
                                        {0,0,0,0,0,0,0,0,0}
                                        },
                                        {{0,0,0,0,0,0,0,0,0},
                                        {0,0,0,0,0,0,0,0,0},
                                        {0,0,0,0,0,0,0,0,0},
                                        {1,0,0,0,1,0,0,0,1},
                                        {0,1,0,1,1,1,0,1,0},
                                        {1,0,0,0,1,0,0,0,1},
                                        {0,0,0,0,0,0,0,0,0},
                                        {0,0,0,0,0,0,0,0,0},
                                        {0,0,0,0,0,0,0,0,0}
                                        },
                                        {{0,0,0,0,0,0,0,0,0},
                                        {0,0,0,0,0,0,0,0,0},
                                        {0,0,0,0,0,0,0,0,0},
                                        {1,0,0,0,1,0,0,0,1},
                                        {1,1,1,1,1,1,1,1,1},
                                        {1,0,0,0,1,0,0,0,1},
                                        {0,0,0,0,0,0,0,0,0},
                                        {0,0,0,0,0,0,0,0,0},
                                        {0,0,0,0,0,0,0,0,0}
                                        },
                                        {{0,0,0,0,0,0,0,0,0},
                                        {0,0,0,0,0,0,0,0,0},
                                        {0,0,0,0,0,0,0,0,0},
                                        {0,0,0,0,0,0,0,0,0},
                                        {0,0,0,0,0,0,0,0,0},
                                        {1,0,0,1,0,1,0,0,1},
                                        {0,1,1,0,0,0,1,1,0},
                                        {0,1,1,0,0,0,1,1,0},
                                        {1,0,0,1,0,1,0,0,1}
                                        },
                                       {{0,0,0,0,0,0,0,0,0},
                                        {0,1,0,0,0,0,0,1,0},
                                        {0,0,1,0,1,0,1,0,0},
                                        {0,0,0,1,0,1,0,0,0},
                                        {0,0,1,0,1,0,1,0,0},
                                        {0,0,0,1,0,1,0,0,0},
                                        {0,0,1,0,1,0,1,0,0},
                                        {0,1,0,0,0,0,0,1,0},
                                        {0,0,0,0,0,0,0,0,0}
                                        },
                                       {{0,0,0,0,0,0,0,0,0},
                                        {0,0,0,0,1,0,0,0,0},
                                        {0,0,1,0,1,0,1,0,0},
                                        {0,0,0,1,1,1,0,0,0},
                                        {0,1,1,1,1,1,1,1,0},
                                        {0,0,0,1,1,1,0,0,0},
                                        {0,0,1,0,1,0,1,0,0},
                                        {0,0,0,0,1,0,0,0,0},
                                        {0,0,0,0,0,0,0,0,0}
                                        },
                                        {{0,0,0,0,0,0,0,0,0},
                                        {0,0,0,0,0,0,0,0,0},
                                        {1,1,0,0,0,0,0,1,1},
                                        {1,1,0,0,0,0,0,1,1},
                                        {1,0,1,0,0,0,1,0,1},
                                        {1,0,0,1,0,1,0,0,1},
                                        {1,0,0,0,1,0,0,0,1},
                                        {1,0,0,0,1,0,0,0,1},
                                        {0,0,0,0,0,0,0,0,0}
                                        },
                                       {{0,0,0,0,0,0,0,0,0},
                                        {0,0,0,1,0,1,0,0,0},
                                        {0,0,1,0,0,0,1,0,0},
                                        {0,1,0,0,1,0,0,1,0},
                                        {0,0,0,1,1,1,0,0,0},
                                        {0,1,0,0,1,0,0,1,0},
                                        {0,0,1,0,0,0,1,0,0},
                                        {0,0,0,1,0,1,0,0,0},
                                        {0,0,0,0,0,0,0,0,0}
                                        },
                                       {{0,0,0,0,0,0,0,0,0},
                                        {0,0,0,0,0,0,0,0,0},
                                        {0,0,0,0,0,0,0,0,0},
                                        {0,0,0,0,0,0,0,0,0},
                                        {1,0,1,0,1,0,1,0,1},
                                        {0,0,0,0,0,0,0,0,0},
                                        {1,0,1,0,1,0,1,0,1},
                                        {0,0,0,0,0,0,0,0,0},
                                        {1,0,1,0,1,0,1,0,1}
                                        },
                                       {{0,0,0,0,0,0,0,0,0},
                                        {0,0,0,0,0,0,0,0,0},
                                        {0,0,0,0,0,0,0,0,0},
                                        {0,0,0,0,0,0,0,0,0},
                                        {1,0,0,0,0,0,0,0,1},
                                        {0,1,0,0,0,0,0,1,0},
                                        {0,0,1,1,1,1,1,0,0},
                                        {0,1,0,0,0,0,0,1,0},
                                        {1,0,0,0,0,0,0,0,1}
                                        },
                                       {{0,0,0,0,0,0,0,0,0},
                                        {0,0,0,0,1,0,0,0,0},
                                        {0,0,0,0,1,0,0,0,0},
                                        {0,0,0,0,1,0,0,0,0},
                                        {0,1,0,0,1,0,0,1,0},
                                        {0,0,1,0,1,0,1,0,0},
                                        {0,0,0,1,1,1,0,0,0},
                                        {0,0,0,0,1,0,0,0,0},
                                        {0,0,0,0,0,0,0,0,0}
                                        },
                                       {{0,0,0,1,1,1,0,0,0},
                                        {0,0,1,0,0,0,1,0,0},
                                        {0,0,1,0,0,0,1,0,0},
                                        {0,0,0,1,1,1,0,0,0},
                                        {0,0,0,0,1,0,0,0,0},
                                        {0,0,0,0,1,0,0,0,0},
                                        {0,0,1,0,1,0,1,0,0},
                                        {0,0,0,1,1,1,0,0,0},
                                        {0,0,0,0,1,0,0,0,0}
                                        },
                                       {{0,0,0,1,1,1,0,0,0},
                                        {0,0,1,0,0,0,1,0,0},
                                        {0,0,1,0,0,0,1,0,0},
                                        {0,0,0,1,1,1,0,0,0},
                                        {0,0,0,0,1,0,0,0,0},
                                        {0,0,1,1,1,1,1,0,0},
                                        {0,0,0,0,1,0,0,0,0},
                                        {0,0,0,0,1,0,0,0,0},
                                        {0,0,0,0,1,0,0,0,0}
                                        },
                                       {{0,0,0,0,0,0,0,0,0},
                                        {0,0,0,0,1,1,0,0,0},
                                        {0,0,0,0,0,1,1,0,0},
                                        {0,0,0,0,0,0,1,1,0},
                                        {1,1,1,1,1,1,1,1,1},
                                        {1,1,1,1,1,1,1,1,1},
                                        {0,0,0,0,0,0,1,1,0},
                                        {0,0,0,0,0,1,1,0,0},
                                        {0,0,0,0,1,1,0,0,0}
                                        },
                                       {{0,0,0,0,0,0,0,0,0},
                                        {0,0,0,0,0,0,0,0,0},
                                        {0,0,0,0,0,0,0,0,0},
                                        {0,0,0,0,0,0,0,0,0},
                                        {1,1,0,1,1,0,1,0,0},
                                        {0,1,1,0,1,1,0,1,0},
                                        {0,0,1,1,0,1,1,0,1},
                                        {0,1,1,0,1,1,0,1,0},
                                        {1,1,0,1,1,0,1,0,0}
                                        },
                                     };

                                        /*
                                        {{0,0,0,0,0,0,0,0,0},
                                        {0,0,0,0,0,0,0,0,0},
                                        {0,0,0,0,0,0,0,0,0},
                                        {0,0,0,0,0,0,0,0,0},
                                        {0,0,0,0,0,0,0,0,0},
                                        {0,0,0,0,0,0,0,0,0},
                                        {0,0,0,0,0,0,0,0,0},
                                        {0,0,0,0,0,0,0,0,0},
                                        {0,0,0,0,0,0,0,0,0}
                                        },
                                        */
    }
}
