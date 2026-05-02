function getAntiForgeryToken() {
    const el = document.querySelector('input[name="__RequestVerificationToken"]');
    return el ? el.value : null;
}

const name = document.querySelector('#title');
const diet = document.querySelector('#diet');

const avTitle = document.querySelector('#available-title');
const exTitle = document.querySelector('#existing-title');
const available = document.querySelector('#available');
const existing = document.querySelector('#existing');

function onSpecieSelect(data) {
    name.textContent = data.name;
    diet.textContent = data.classification.label;

    document.querySelector('#diet-badge').style.display = 'inline-block';

    renderAvailablePrey(data.available, data.id);
    renderExistingPrey(data.preys, data.id);
}

function renderExistingPrey(preys, rootSpecie) {
    existing.innerHTML = '';
    if (!preys || preys.length === 0) {
        exTitle.textContent = 'Chaîne isolée';
        exTitle.style.color = 'var(--text-muted)';
        return;
    }

    exTitle.textContent = "Proies actuelles";
    exTitle.style.color = 'var(--text-bright)';

    for (const p of preys) {
        const li = document.createElement('li');
        li.className = "li-info-card";
        li.innerHTML = `
            <div class="d-flex flex-column">
                <span>${p.name}</span>
                <span class="li-card-meta">${p.classification.label}</span>
            </div>
            <button class="btn-action btn-remove" title="Supprimer le lien" 
                    onclick="handleAction('${rootSpecie}', '${p.specieId}', 'remove')">×</button>
        `;
        existing.appendChild(li);
    }
}

function renderAvailablePrey(preys, rootSpecie) {
    available.innerHTML = '';
    if (!preys || preys.length === 0) {
        avTitle.textContent = 'Aucune proie compatible';
        avTitle.style.color = 'var(--text-muted)';
        return;
    }

    avTitle.textContent = "Suggestions de proies";
    avTitle.style.color = 'var(--text-bright)';

    for (const p of preys) {
        const li = document.createElement('li');
        li.className = "li-info-card";
        li.innerHTML = `
            <div class="d-flex flex-column">
                <span>${p.name}</span>
                <span class="li-card-meta">${p.classification.label}</span>
            </div>
            <button class="btn-action btn-add" title="Ajouter le lien" 
                    onclick="handleAction('${rootSpecie}', '${p.specieId}', 'add')">+</button>
        `;
        available.appendChild(li);
    }
}

function handleAction(predator, prey, action) {
    const url = action === 'add' ? '?handler=AddRelation' : '?handler=RemoveRelation';

    fetch(url, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'RequestVerificationToken': getAntiForgeryToken()
        },
        body: JSON.stringify({ predatorId: parseInt(predator), preyId: parseInt(prey) })
    })
        .then(res => {
            if (res.ok) {
                location.reload();
            }
        })
        .catch(err => console.error("Erreur d'action:", err));
}

(function () {
    const data = window.foodChainData;
    const isAdmin = window.isAdmin;

    if (!data) {
        console.error("foodChainData is missing");
        return;
    }

    const nodes = data.Nodes || [];
    const edges = data.Edges || [];

    const elements = [
        ...nodes.map(n => ({
            data: {
                id: n.SpecieId,
                label: `${n.Name}\n${n.Diet.Label}`
            }
        })),
        ...edges.map(e => ({
            data: {
                source: e.PredatorId,
                target: e.PreyId
            }
        }))
    ];

    const cy = cytoscape({
        container: document.getElementById('cy'),

        elements,

        style: [

            {
                selector: 'node',
                style: {
                    'background-color': '#132830',
                    'label': 'data(label)',
                    'color': '#e8f4f2',
                    'font-family': 'var(--sans)',
                    'font-size': '13px',
                    'font-weight': '500',
                    'text-valign': 'center',
                    'text-halign': 'center',
                    'text-wrap': 'wrap',
                    'text-max-width': 90,
                    'line-height': 1.2,

                    'border-width': 1,
                    'border-color': 'rgba(0, 210, 180, 0.25)',

                    'width': 70,
                    'height': 70,

                    'text-outline-width': 2,
                    'text-outline-color': '#060d0f'
                }
            },

            {
                selector: 'edge',
                style: {
                    'width': 2,
                    'line-color': 'rgba(122, 173, 168, 0.35)',
                    'target-arrow-color': 'rgba(0, 210, 180, 0.6)',
                    'target-arrow-shape': 'triangle',
                    'curve-style': 'bezier',
                    'arrow-scale': 1.1,
                    'opacity': 0.9
                }
            },

            {
                selector: 'node:hover',
                style: {
                    'border-color': 'rgba(0, 210, 180, 0.8)',
                    'border-width': 2,
                    'background-color': '#0f2229'
                }
            },

            {
                selector: 'edge:hover',
                style: {
                    'line-color': '#00d2b4',
                    'target-arrow-color': '#00d2b4',
                    'width': 3,
                    'opacity': 1
                }
            },

            {
                selector: ':selected',
                style: {
                    'border-color': '#00d2b4',
                    'border-width': 3
                }
            }
        ],

        layout: {
            name: 'breadthfirst',
            directed: true,
            padding: 40,
            spacingFactor: 1.7,
            animate: false,
        }
    });
    
    if (isAdmin) {
        cy.on('tap', 'node', function (evt) {
            const node = evt.target;

            fetch('?handler=NodeClicked', {

                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'RequestVerificationToken': getAntiForgeryToken()
                },
                body: JSON.stringify({ specieId: parseInt(node.id()) })
            })
                .then(res => res.json())
                .then(data => {
                    console.log(JSON.stringify(data));
                    onSpecieSelect(data);
                })
        });
    }

    setTimeout(() => {
        cy.resize();
        cy.fit();
    }, 50);

    window.addEventListener('resize', () => {
        cy.resize();
    });

})();
