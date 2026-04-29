function getAntiForgeryToken() {
    const el = document.querySelector('input[name="__RequestVerificationToken"]');
    return el ? el.value : null;
}

const name = document.querySelector('#title');
const diet = document.querySelector('#diet');
const available = document.querySelector('#available');

function onSpecieSelect(data) {
    name.textContent = data.name;
    diet.textContent = data.classification.label;
    renderAvailablePrey(data.available);
}

function renderAvailablePrey(preys) {
    // reset
    available.innerHTML = '';

    if (!preys || preys.length === 0) {
        const empty = document.createElement('li');
        empty.textContent = 'Aucune proie disponible';
        empty.className = 'text-muted';
        available.appendChild(empty);
        return;
    }
    
    const header = document.createElement('p');
    header.textContent = "Proies disponible:";
    available.appendChild(header);

    // insert
    for (const p of preys) {
        const li = document.createElement('li');

        li.textContent = `${p.name} (${p.classification.label})`;
        li.dataset.slug = p.slug;

        li.classList.add('list-group-item', 'bg-transparent', 'text-light');

        available.appendChild(li);
    }
}

(function () {
    const data = window.foodChainData;

    if (!data) {
        console.error("foodChainData is missing");
        return;
    }

    const nodes = data.Nodes || [];
    const edges = data.Edges || [];

    const elements = [
        ...nodes.map(n => ({
            data: {
                id: n.Slug,
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
            animate: true,
            animationDuration: 400
        }
    });
    
    cy.on('tap', 'node', function (evt) {
        const node = evt.target;

        fetch('/food-chain?handler=NodeClicked', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'RequestVerificationToken': getAntiForgeryToken()
            },
            body: JSON.stringify({
                slug: node.id()
            })
        })
            .then(res => res.json())
            .then(data => {
                console.log(JSON.stringify(data));
                onSpecieSelect(data);
            })
    });

    setTimeout(() => {
        cy.resize();
        cy.fit();
    }, 50);

    window.addEventListener('resize', () => {
        cy.resize();
    });

})();
