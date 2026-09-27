import { useState, type FormEvent } from 'react';
import {
  Activity, ArrowDownToLine, ArrowUpRight, Boxes, Building2, CheckCircle2,
  ChevronDown, CircleHelp, Command, Cpu, LayoutDashboard, Plus, Search,
  Server, Settings2, ShieldCheck, SquareTerminal, UsersRound, X
} from 'lucide-react';

type Section = 'Overview' | 'Organizations' | 'Projects' | 'Workspaces' | 'Users' | 'Settings';
type Organization = { name: string; slug: string; plan: string; status: string; projects: number };

const nav = [
  { name: 'Overview', icon: LayoutDashboard }, { name: 'Organizations', icon: Building2 },
  { name: 'Projects', icon: Boxes }, { name: 'Workspaces', icon: SquareTerminal },
  { name: 'Users', icon: UsersRound }, { name: 'Settings', icon: Settings2 }
] as const;
const seed: Organization[] = [
  { name: 'Northstar Labs', slug: 'northstar', plan: 'Scale', status: 'Active', projects: 12 },
  { name: 'Fieldwork Studio', slug: 'fieldwork', plan: 'Team', status: 'Active', projects: 8 },
  { name: 'Aperture Systems', slug: 'aperture', plan: 'Growth', status: 'Trial', projects: 3 },
  { name: 'Common Thread', slug: 'common-thread', plan: 'Team', status: 'Active', projects: 6 },
  { name: 'Morrow Works', slug: 'morrow', plan: 'Starter', status: 'Suspended', projects: 2 }
];
const projects = [
  ['Atlas API', 'Northstar Labs', '.NET 10 · PostgreSQL', 'Healthy'],
  ['Field Notes', 'Fieldwork Studio', 'React · Node.js', 'Healthy'],
  ['Lens Pipeline', 'Aperture Systems', 'Python · Redis', 'Building'],
  ['Thread Console', 'Common Thread', 'Next.js · PostgreSQL', 'Healthy']
];
const workspaces = [
  ['atlas-api-dev', 'Atlas API', 'Maya Chen', 'US East', 'Running'],
  ['field-notes-preview', 'Field Notes', 'Leo Park', 'EU West', 'Running'],
  ['lens-pipeline-build', 'Lens Pipeline', 'Nora Ali', 'US West', 'Starting']
];
const descriptions: Record<Section, string> = {
  Overview: 'A live snapshot of the teams and compute running on your platform.',
  Organizations: 'Manage customer accounts, plans, and access status.',
  Projects: 'Browse projects across every organization.',
  Workspaces: 'Monitor development environments and runner health.',
  Users: 'Review people and their organization access.',
  Settings: 'Platform configuration and operational defaults.'
};

function Status({ value }: { value: string }) {
  return <span className={`status status-${value.toLowerCase()}`}><i />{value}</span>;
}

export default function Portal() {
  const [section, setSection] = useState<Section>('Overview');
  const [organizations, setOrganizations] = useState(seed);
  const [search, setSearch] = useState('');
  const [showCreate, setShowCreate] = useState(false);
  const [notice, setNotice] = useState('');
  const visibleOrganizations = organizations.filter((item) =>
    `${item.name} ${item.slug} ${item.plan}`.toLowerCase().includes(search.toLowerCase())
  );

  function createOrganization(event: FormEvent<HTMLFormElement>) {
    event.preventDefault();
    const data = new FormData(event.currentTarget);
    const name = String(data.get('name')).trim();
    const slug = String(data.get('slug')).trim().toLowerCase();
    if (!name || !slug) return;
    setOrganizations((items) => [{ name, slug, plan: String(data.get('plan')), status: 'Trial', projects: 0 }, ...items]);
    setShowCreate(false);
    setNotice(`${name} added to the local preview`);
    window.setTimeout(() => setNotice(''), 3000);
  }

  function exportOrganizations() {
    const rows = ['Name,Slug,Plan,Status,Projects', ...organizations.map((item) =>
      [item.name, item.slug, item.plan, item.status, item.projects].map((value) => `"${String(value).replace(/"/g, '""')}"`).join(',')
    )];
    const url = URL.createObjectURL(new Blob([rows.join('\n')], { type: 'text/csv' }));
    const link = document.createElement('a');
    link.href = url;
    link.download = 'organizations.csv';
    link.click();
    URL.revokeObjectURL(url);
  }

  return <div className="portal-shell">
    <aside className="sidebar">
      <div className="brand"><span className="brand-mark"><Command size={18} /></span><span><strong>mydev</strong><small>PLATFORM</small></span></div>
      <div className="tenant-switch"><span className="tenant-mark">MP</span><span><strong>MyDev Platform</strong><small>Superadmin</small></span><ChevronDown size={15} /></div>
      <p className="nav-caption">WORKSPACE</p>
      <nav className="nav" aria-label="Main navigation">{nav.map(({ name, icon: Icon }) => <button key={name} className={`nav-item${section === name ? ' active' : ''}`} onClick={() => setSection(name)}><Icon size={17} /><span>{name}</span>{name === 'Organizations' && <small>{organizations.length}</small>}</button>)}</nav>
      <div className="sidebar-spacer" />
      <div className="system-card"><div className="system-title"><i />All systems operational</div><div className="capacity-label"><span>Runner capacity</span><strong>68%</strong></div><div className="capacity-track"><i /></div><button onClick={() => setSection('Workspaces')}>System status <ArrowUpRight size={13} /></button></div>
      <div className="profile"><span className="avatar avatar-admin">JD</span><span><strong>Jordan Davis</strong><small>Platform admin</small></span><ChevronDown size={15} /></div>
    </aside>

    <main className="main-panel">
      <header className="topbar"><div className="breadcrumbs">MyDev Platform <span>/</span> <strong>{section}</strong></div><div className="top-tools"><label className="search-box"><Search size={16} /><input aria-label="Search organizations" placeholder="Search organizations" value={search} onChange={(event) => setSearch(event.target.value)} /><kbd>⌘ K</kbd></label><button className="icon-button" aria-label="Help"><CircleHelp size={18} /></button><button className="icon-button" aria-label="Activity"><Activity size={18} /></button><span className="avatar avatar-admin">JD</span></div></header>
      <div className="page-content">
        <section className="page-heading"><div><p className="eyebrow">CONTROL PLANE / {section.toUpperCase()}</p><h1>{section === 'Overview' ? 'Platform overview' : section}</h1><p>{descriptions[section]}</p></div><div className="heading-actions"><button className="button button-secondary" onClick={exportOrganizations}><ArrowDownToLine size={16} />Export</button><button className="button button-primary" onClick={() => setShowCreate(true)}><Plus size={17} />New organization</button></div></section>

        {section === 'Overview' && <>
          <section className="stats-grid">{[
            ['Organizations', String(organizations.length).padStart(2, '0'), Building2, 'mint'],
            ['Active projects', '118', Boxes, 'blue'], ['Live workspaces', '41', SquareTerminal, 'orange'], ['Platform uptime', '99.98%', Activity, 'gold']
          ].map(([label, value, Icon, tone]) => <article className="stat-card" key={String(label)}><span className={`stat-icon ${tone}`}><Icon size={18} /></span><small>{String(label)}</small><strong>{String(value)}</strong><span className="stat-foot"><i />{label === 'Organizations' ? '+3 this month' : label === 'Live workspaces' ? '36 running now' : 'Last 30 days'}</span></article>)}</section>
          <section className="overview-grid">
            <article className="surface"><div className="surface-heading"><div><h2>Organizations</h2><p>Recently active customer teams</p></div><button className="text-button" onClick={() => setSection('Organizations')}>View all <ArrowUpRight size={15} /></button></div><OrgTable organizations={organizations.slice(0, 4)} /></article>
            <article className="surface activity-panel"><div className="surface-heading"><div><h2>Platform activity</h2><p>Recent events across your platform</p></div><Activity size={17} /></div>{[
              ['Maya Chen', 'created a workspace', 'atlas-api-dev', '8 min ago'], ['Aperture Systems', 'started a trial', 'Growth plan', '1 hr ago'], ['Leo Park', 'deployed a project', 'Field Notes', '3 hr ago'], ['Platform', 'completed maintenance', 'US East runners', 'Yesterday']
            ].map(([actor, action, target, time]) => <div className="activity-row" key={target}><span className="activity-check"><CheckCircle2 size={16} /></span><p><strong>{actor}</strong> {action}<small>{target}</small></p><time>{time}</time></div>)}</article>
          </section>
          <section className="lower-grid"><article className="surface usage-panel"><div className="surface-heading"><div><h2>Compute usage</h2><p>Shared runner capacity this week</p></div><button className="period-select">7 days <ChevronDown size={14} /></button></div><div className="usage-total">68% <small>+8.4% from last week</small></div><div className="usage-bars">{[38, 54, 47, 71, 63, 82, 68].map((height, index) => <div key={index}><i style={{ height: `${height}%` }} /><small>{'MTWTFSS'[index]}</small></div>)}</div></article><article className="surface health-panel"><div className="surface-heading"><div><h2>Infrastructure</h2><p>Core service health</p></div><button className="text-button" onClick={() => setSection('Workspaces')}>Details <ArrowUpRight size={15} /></button></div>{[[Server, 'Workspace runners', 'US East, EU West'], [Cpu, 'API response time', 'p95 · 142 ms'], [Activity, 'Queue wait time', 'Average · 18 sec']].map(([Icon, label, detail]) => <div className="health-row" key={String(label)}><span><Icon size={16} /></span><p><strong>{String(label)}</strong><small>{String(detail)}</small></p><Status value="Healthy" /></div>)}</article></section>
        </>}

        {section === 'Organizations' && <section className="surface table-surface"><div className="surface-heading"><div><h2>All organizations <span className="count">{visibleOrganizations.length}</span></h2><p>Accounts provisioned on MyDev Platform</p></div><button className="button button-primary" onClick={() => setShowCreate(true)}><Plus size={16} />New organization</button></div>{visibleOrganizations.length ? <OrgTable organizations={visibleOrganizations} expanded /> : <div className="empty-state"><Search size={22} /><strong>No organizations found</strong><span>Try another name or plan.</span></div>}<div className="table-footer">Showing {visibleOrganizations.length} organizations <span>Page 1 of 1</span></div></section>}

        {section === 'Projects' && <section className="surface table-surface"><div className="surface-heading"><div><h2>All projects <span className="count">118</span></h2><p>Projects from active organizations</p></div></div><div className="table-wrap"><table><thead><tr><th>PROJECT</th><th>ORGANIZATION</th><th>STACK</th><th>STATUS</th><th>UPDATED</th></tr></thead><tbody>{projects.map(([name, org, stack, status]) => <tr key={name}><td><strong>{name}</strong></td><td>{org}</td><td>{stack}</td><td><Status value={status} /></td><td>12 min ago</td></tr>)}</tbody></table></div><div className="table-footer">Showing 4 of 118 projects <span>Page 1 of 30</span></div></section>}

        {section === 'Workspaces' && <section className="workspace-grid">{workspaces.map(([name, project, owner, region, status]) => <article className="workspace-card" key={name}><div className="workspace-icon"><SquareTerminal size={19} /><ArrowUpRight size={15} /></div><h2>{name}</h2><p>{project}</p><div className="workspace-rule" /><div className="workspace-meta"><span>Owner</span><strong>{owner}</strong></div><div className="workspace-meta"><span>Region</span><strong>{region}</strong></div><div className="workspace-bottom"><Status value={status} /><button className="text-button">Open terminal <ArrowUpRight size={14} /></button></div></article>)}</section>}

        {section === 'Users' && <section className="surface table-surface"><div className="surface-heading"><div><h2>Platform users <span className="count">246</span></h2><p>People with access to customer organizations</p></div></div><div className="table-wrap"><table><thead><tr><th>USER</th><th>ORGANIZATION</th><th>ROLE</th><th>LAST ACTIVE</th></tr></thead><tbody>{[['Maya Chen', 'maya@northstar.dev', 'Northstar Labs', 'Owner', 'MC'], ['Leo Park', 'leo@fieldwork.studio', 'Fieldwork Studio', 'Admin', 'LP'], ['Nora Ali', 'nora@aperture.io', 'Aperture Systems', 'Developer', 'NA']].map(([name, email, org, role, initials]) => <tr key={email}><td><div className="user-cell"><span className="avatar">{initials}</span><span><strong>{name}</strong><small>{email}</small></span></div></td><td>{org}</td><td>{role}</td><td>Today</td></tr>)}</tbody></table></div><div className="table-footer">Showing 3 of 246 users <span>Page 1 of 82</span></div></section>}

        {section === 'Settings' && <section className="settings-grid">{[[ShieldCheck, 'Access policies', 'Default session and sign-in requirements for platform administrators.', 'Multi-factor authentication', 'Enabled'], [Server, 'Workspace defaults', 'Default region and resource limits for newly provisioned workspaces.', 'Default region', 'US East · Virginia'], [Activity, 'Audit retention', 'Retention window applied to platform and organization audit events.', 'Event history', '90 days']].map(([Icon, title, description, label, value]) => <article className="surface settings-card" key={String(title)}><span className="settings-icon"><Icon size={18} /></span><h2>{String(title)}</h2><p>{String(description)}</p><div><span>{String(label)}</span><strong>{String(value)}</strong></div></article>)}</section>}
        <footer className="page-foot">Platform control plane <span>Region: us-east-1</span><span>Updated just now <i /></span></footer>
      </div>
    </main>

    {notice && <div className="toast"><CheckCircle2 size={18} />{notice}</div>}
    {showCreate && <div className="modal-backdrop" onMouseDown={(event) => { if (event.target === event.currentTarget) setShowCreate(false); }}><section className="modal" role="dialog" aria-modal="true" aria-labelledby="create-title"><div className="modal-heading"><div><p className="eyebrow">NEW TENANT</p><h2 id="create-title">Create organization</h2></div><button className="icon-button" aria-label="Close" onClick={() => setShowCreate(false)}><X size={18} /></button></div><form onSubmit={createOrganization}><label>Organization name<input name="name" placeholder="Northstar Labs" required autoFocus /></label><label>Workspace slug<input name="slug" placeholder="northstar" pattern="[a-zA-Z0-9-]+" required /></label><label>Subscription plan<select name="plan" defaultValue="Team"><option>Starter</option><option>Team</option><option>Growth</option><option>Scale</option></select></label><div className="modal-actions"><button type="button" className="button button-secondary" onClick={() => setShowCreate(false)}>Cancel</button><button type="submit" className="button button-primary"><Plus size={16} />Create organization</button></div></form><p className="modal-note"><ShieldCheck size={14} /> Preview only. This does not write to the API yet.</p></section></div>}
  </div>;
}

function OrgTable({ organizations, expanded = false }: { organizations: Organization[]; expanded?: boolean }) {
  return <div className="table-wrap"><table><thead><tr><th>ORGANIZATION</th><th>PLAN</th><th>STATUS</th><th>PROJECTS</th>{expanded && <th>CREATED</th>}<th /></tr></thead><tbody>{organizations.map((item) => <tr key={item.slug}><td><div className="org-cell"><span className="org-monogram">{item.name.split(' ').map((part) => part[0]).slice(0, 2).join('')}</span><span><strong>{item.name}</strong><small>{item.slug}.mydev.app</small></span></div></td><td><span className="plan-label">{item.plan}</span></td><td><Status value={item.status} /></td><td>{item.projects}</td>{expanded && <td>Sep 18, 2026</td>}<td><button className="row-action" aria-label={`Open ${item.name}`}><ArrowUpRight size={15} /></button></td></tr>)}</tbody></table></div>;
}
