const stats = [
  { label: 'Organizations', value: '24', trend: '+3.4%' },
  { label: 'Active Projects', value: '118', trend: '+12.1%' },
  { label: 'Running Workspaces', value: '41', trend: '+8.0%' },
  { label: 'Subscription Health', value: '96%', trend: 'stable' }
];

const nav = ['Dashboard', 'Organizations', 'Projects', 'Workspace', 'Users', 'Templates', 'Servers'];

const columns = [
  { title: 'Recent activity', items: ['Org: Northwind launched', 'Project: Platform API updated', 'User: 3 invites sent'] },
  { title: 'System status', items: ['PostgreSQL healthy', 'API latency 120ms', 'Workspace runner online'] },
  { title: 'Subscription alerts', items: ['2 plans expiring soon', '1 trial about to convert', '2 suspended orgs'] }
];

export default function App() {
  return (
    <div className="app-shell">
      <aside className="sidebar">
        <div className="brand">
          <div className="brand-mark">M</div>
          <div>
            <h1>MyDevPlatform</h1>
            <small>Superadmin</small>
          </div>
        </div>

        <nav className="nav">
          {nav.map((item, index) => (
            <button key={item} className={index === 0 ? 'nav-item active' : 'nav-item'}>
              {item}
            </button>
          ))}
        </nav>

        <div className="sidebar-card">
          <p className="eyebrow">Environment</p>
          <strong>Production</strong>
          <span>Platform health: good</span>
        </div>
      </aside>

      <main className="main-panel">
        <header className="topbar">
          <div>
            <p className="eyebrow">Overview</p>
            <h2>Platform dashboard</h2>
          </div>
          <div className="topbar-actions">
            <button className="secondary">Export</button>
            <button className="primary">Create organization</button>
          </div>
        </header>

        <section className="stats-grid">
          {stats.map((stat) => (
            <article key={stat.label} className="stat-card">
              <span>{stat.label}</span>
              <strong>{stat.value}</strong>
              <em>{stat.trend}</em>
            </article>
          ))}
        </section>

        <section className="content-grid">
          {columns.map((column) => (
            <article key={column.title} className="panel-card">
              <h3>{column.title}</h3>
              <ul>
                {column.items.map((item) => (
                  <li key={item}>{item}</li>
                ))}
              </ul>
            </article>
          ))}
        </section>

        <section className="workspace-panel">
          <div className="workspace-header">
            <h3>Project workspace preview</h3>
            <button className="secondary">Open workspace</button>
          </div>

          <div className="workspace-layout">
            <aside className="explorer">
              <div className="explorer-title">Explorer</div>
              <ul>
                <li>src/</li>
                <li>app/</li>
                <li>components/</li>
                <li>config/</li>
              </ul>
            </aside>

            <div className="editor">
              <div className="editor-tabs">
                <span className="tab active">Program.cs</span>
                <span className="tab">appsettings.json</span>
              </div>
              <pre>{`public class Program
{
    public static void Main()
    {
        Console.WriteLine("MyDevPlatform ready");
    }
}`}</pre>
            </div>

            <aside className="project-panel">
              <div className="panel-block">
                <h4>Git</h4>
                <p>main</p>
              </div>
              <div className="panel-block">
                <h4>Run</h4>
                <p>dotnet build</p>
              </div>
              <div className="panel-block">
                <h4>Debug</h4>
                <p>Ready</p>
              </div>
            </aside>
          </div>
        </section>
      </main>
    </div>
  );
}
