const dataById = Object.fromEntries(rawData.map(b => [b.id, b]));

const fuse = new Fuse(rawData, {
    keys: [
        {name: 'name', weight: 0.35},
        {name: 'species', weight: 0.3},
        {name: 'equipments', weight: 0.3},
        {name: 'temperature', weight: 0.025},
        {name: 'humidity', weight: 0.025},
    ],
    threshold: 0.3,
    minMatchCharLength: 1,
    includeScore: true,
});

const input = document.getElementById('biome-search');
const count = document.getElementById('search-count');

input.addEventListener('input', () => {
    const q = input.value.trim();

    document.querySelectorAll('.biome-row-wrap').forEach(row => {
        row.style.display = '';
        row.querySelector('.search-meta-tags')?.remove();
    });
    document.querySelectorAll('.card-body').forEach(s => s.style.display = '');
    count.style.display = 'none';

    if (!q) return;

    const matchedIds = new Set(fuse.search(q).map(r => r.item.id));

    document.querySelectorAll('.biome-row-wrap').forEach(row => {
        const id = parseInt(row.dataset.id);
        if (!id) return;

        if (!matchedIds.has(id)) {
            row.style.display = 'none';
            return;
        }

        const b = dataById[id];
        if (!b) return;

        const species = (b.species || []).map(s => `<span class="search-tag species">${s}</span>`).join('');
        const equipments = (b.equipments || []).map(e => `<span class="search-tag equipment">${e}</span>`).join('');
        if (!species && !equipments) return;

        const tagsEl = document.createElement('div');
        tagsEl.className = 'search-meta-tags';
        tagsEl.innerHTML = species + equipments;
        row.appendChild(tagsEl);
    });

    document.querySelectorAll('.card-body').forEach(section => {
        const visible = [...section.querySelectorAll('.biome-row-wrap')]
            .some(r => r.style.display !== 'none');
        section.style.display = visible ? '' : 'none';
    });

    count.style.display = 'block';
    count.textContent = `${matchedIds.size} résultat(s) pour "${q}"`;
});