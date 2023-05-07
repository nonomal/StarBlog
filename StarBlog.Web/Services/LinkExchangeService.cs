﻿using System.Text;
using FreeSql;
using Microsoft.Extensions.Options;
using StarBlog.Data.Models;
using StarBlog.Share.Utils;

namespace StarBlog.Web.Services;

/// <summary>
/// 友情链接申请
/// </summary>
public class LinkExchangeService {
    private readonly IBaseRepository<LinkExchange> _repo;
    private readonly LinkService _linkService;
    private readonly EmailAccountConfig _emailAccountConfig;

    public LinkExchangeService(IBaseRepository<LinkExchange> repo, LinkService linkService, IOptions<EmailAccountConfig> options) {
        _repo = repo;
        _linkService = linkService;
        _emailAccountConfig = options.Value;
    }

    /// <summary>
    /// 查询 id 是否存在
    /// </summary>
    public async Task<bool> HasId(int id) {
        return await _repo.Where(a => a.Id == id).AnyAsync();
    }

    public async Task<bool> HasUrl(string url) {
        return await _repo.Where(a => a.Url.Contains(url)).AnyAsync();
    }

    public async Task<List<LinkExchange>> GetAll() {
        return await _repo.Select.ToListAsync();
    }

    public async Task<LinkExchange?> GetById(int id) {
        return await _repo.Where(a => a.Id == id).FirstAsync();
    }

    public async Task<LinkExchange> AddOrUpdate(LinkExchange item) {
        return await _repo.InsertOrUpdateAsync(item);
    }

    public async Task<LinkExchange?> SetVerifyStatus(int id, bool status, string? reason = null) {
        var item = await GetById(id);
        if (item == null) return null;

        item.Verified = status;
        item.Reason = reason;
        await _repo.UpdateAsync(item);


        var link = await _linkService.GetByName(item.Name);
        if (status) {
            await SendEmailOnAccept(item);
            if (link == null) {
                await _linkService.AddOrUpdate(new Link {
                    Name = item.Name,
                    Description = item.Description,
                    Url = item.Url,
                    Visible = true
                });
            }
            else {
                await _linkService.SetVisibility(link.Id, true);
            }
        }
        else {
            await SendEmailOnReject(item);
            if (link != null) await _linkService.DeleteById(link.Id);
        }

        return await GetById(id);
    }

    public async Task<int> DeleteById(int id) {
        return await _repo.DeleteAsync(a => a.Id == id);
    }

    public async Task SendEmailOnAdd(LinkExchange item) {
        const string starblogLink = "<a href=\"https://deali.cn\">StarBlog</a>";
        var sb = new StringBuilder();
        sb.AppendLine($"<p>友链申请已提交，正在处理中，请及时关注邮件通知~</p>");
        sb.AppendLine($"<br>");
        sb.AppendLine($"<p>以下是您申请的友链信息：</p>");
        sb.AppendLine($"<p>网站名称：{item.Name}</p>");
        sb.AppendLine($"<p>介绍：{item.Description}</p>");
        sb.AppendLine($"<p>网址：{item.Url}</p>");
        sb.AppendLine($"<p>站长：{item.WebMaster}</p>");
        sb.AppendLine($"<br>");
        sb.AppendLine($"<br>");
        sb.AppendLine($"<br>");
        sb.AppendLine($"<p>本消息由 {starblogLink} 自动发送，无需回复。</p>");
        await EmailUtils.SendEmailAsync(
            _emailAccountConfig,
            "[StarBlog]友链申请已提交",
            sb.ToString(),
            item.WebMaster,
            item.Email
        );
    }

    public async Task SendEmailOnAccept(LinkExchange item) {
        const string starblogLink = "<a href=\"https://deali.cn\">StarBlog</a>";
        var sb = new StringBuilder();
        sb.AppendLine($"<p>您好，友链申请已通过！感谢支持，欢迎互访哦~</p>");
        sb.AppendLine($"<br>");
        sb.AppendLine($"<p>以下是您申请的友链信息：</p>");
        sb.AppendLine($"<p>网站名称：{item.Name}</p>");
        sb.AppendLine($"<p>介绍：{item.Description}</p>");
        sb.AppendLine($"<p>网址：{item.Url}</p>");
        sb.AppendLine($"<p>站长：{item.WebMaster}</p>");
        sb.AppendLine($"<p>补充信息：{item.Reason}</p>");
        sb.AppendLine($"<br>");
        sb.AppendLine($"<br>");
        sb.AppendLine($"<br>");
        sb.AppendLine($"<p>本消息由 {starblogLink} 自动发送，无需回复。</p>");
        await EmailUtils.SendEmailAsync(
            _emailAccountConfig,
            "[StarBlog]友链申请结果反馈",
            sb.ToString(),
            item.WebMaster,
            item.Email
        );
    }

    public async Task SendEmailOnReject(LinkExchange item) {
        const string starblogLink = "<a href=\"https://deali.cn\">StarBlog</a>";
        var sb = new StringBuilder();
        sb.AppendLine($"<p>很抱歉，友链申请未通过！建议您查看补充信息，调整后再次进行申请，感谢您的理解与支持~</p>");
        sb.AppendLine($"<br>");
        sb.AppendLine($"<p>以下是您申请的友链信息：</p>");
        sb.AppendLine($"<p>网站名称：{item.Name}</p>");
        sb.AppendLine($"<p>介绍：{item.Description}</p>");
        sb.AppendLine($"<p>网址：{item.Url}</p>");
        sb.AppendLine($"<p>站长：{item.WebMaster}</p>");
        sb.AppendLine($"<p>补充信息：{item.Reason}</p>");
        sb.AppendLine($"<br>");
        sb.AppendLine($"<br>");
        sb.AppendLine($"<br>");
        sb.AppendLine($"<p>本消息由 {starblogLink} 自动发送，无需回复。</p>");
        await EmailUtils.SendEmailAsync(
            _emailAccountConfig,
            "[StarBlog]友链申请结果反馈",
            sb.ToString(),
            item.WebMaster,
            item.Email
        );
    }
}