using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{

    /// <summary>
    /// Api请求状态码
    /// </summary>
    public enum ResultCode
    {
        /// <summary>
        /// 操作成功
        ///</summary>
        [Display(Name = "操作成功", GroupName = Result.SuccessCode)]
        Ok = 0,

        /// <summary>
        /// 操作失败
        ///</summary>
        [Display(Name = "操作失败")]
        Fail = 1,

        /// <summary>
        /// 服务数据异常
        ///</summary>
        [Display(Name = "服务数据异常")]
        ServerError = 10,

        /// <summary>
        /// 未登录
        ///</summary>
        [Display(Name = "未登录")]
        Unauthorized = 20,

        /// <summary>
        /// 未授权
        /// </summary>
        [Display(Name = "未授权")]
        Forbidden = 21,

        /// <summary>
        /// Token 失效
        /// </summary>
        [Display(Name = "Token 失效")]
        InvalidToken = 22,

        /// <summary>
        /// 密码验证失败
        /// </summary>
        [Display(Name = "密码验证失败")]
        SpaFailed = 23,

        /// <summary>
        /// 错误的新密码
        /// </summary>
        [Display(Name = "错误的新密码")]
        WrongNewPassword = 24,

        /// <summary>
        /// 签名验证失败
        /// </summary>
        [Display(Name = "签名验证失败")]
        InvalidSign = 402,

        /// <summary>
        /// 参数验证失败
        /// </summary>
        [Display(Name = "参数验证失败")]
        InvalidData = 403,

        /// <summary>
        /// 没有此条记录
        ///</summary>
        [Display(Name = "没有此条记录")]
        NoRecord = 404,

        /// <summary>
        /// 重复记录
        /// </summary>
        [Display(Name = "已有记录，请勿重复操作")]
        DuplicateRecord = 405,

        /// <summary>
        /// 缺失基础数据
        /// </summary>
        [Display(Name = "缺失基础数据")]
        MissEssentialData = 406,

        /// <summary>
        /// 金额验证失败
        /// </summary>
        [Display(Name = "金额验证失败")]
        InvalidAmount = 407,

        /// <summary>
        /// 缺少对应票类
        /// </summary>
        [Display(Name = "缺少对应票类")]
        MissTicketType = 408,

        /// <summary>
        /// 支付失败
        /// </summary>
        [Display(Name = "支付失败")]
        PayFail = 500,

        /// <summary>
        /// 写入出票记录失败
        /// </summary>
        [Display(Name = "写入出票记录失败")]
        WriteTicketRecordFail = 501,

        /// <summary>
        /// 写入年卡凭证记录失败
        /// </summary>
        [Display(Name = "写入年卡凭证记录失败")]
        WriteVoucherRecordFail = 502,


        /// <summary>
        /// 存在重复发票号或发票号为负数
        /// </summary>
        [Display(Name = "存在重复发票号或发票号为负数")]
        DuplicateInvoiceRecord = 503,

        /// <summary>
        /// 预付款余额已经低于最低限制金额
        /// </summary>
        [Display(Name = "预付款余额已经低于最低限制金额")]
        InsufficientBalance = 504,


        #region 自助售票机接口状态码 1000~2000

        /// <summary>
        /// 订单不存在或已过期
        /// </summary>
        [Display(Name = "订单不存在或已过期")]
        VendorOrderNoExists = 1000,

        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "订单已取票")]
        VendorOrderConsumed = 1001,


        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "余票不足")]
        VendorTicketNotEnough = 1002,

        #endregion

    
    }
}
